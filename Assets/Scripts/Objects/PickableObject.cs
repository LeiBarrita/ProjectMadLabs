using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
    // public event Action OnPick; // Action trigger when pick with no previous holder
    public event Action<IHolder> OnHold; // Action trigger always when pick
    public event Action OnDrop; // Action trigger when drop on the floor
    public event Action<IHolder> OnRelease; // Action trigger when previous holder stops holding

    public IHolder Holder;
    // public bool CanShrink;

    private Transform FollowPosition;
    private Rigidbody rb;

    protected virtual void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.freezeRotation = true;
        rb.isKinematic = false;
    }

    protected virtual void LateUpdate()
    {
        if (FollowPosition == null) return;
        FollowHoldPosition(FollowPosition);
    }

    protected virtual void FollowHoldPosition(Transform followPos)
    {
        transform.SetPositionAndRotation(
            followPos.position,
            followPos.rotation
        );
    }

    public virtual void Pick(NetworkObjectReference playerRef)
    {
        PickItemServerRpc(playerRef);
    }

    public virtual void Drop()
    {
        DropItemServerRpc();
    }

    public virtual void Store()
    {
        // Note: move to client RPC
        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<Collider>().enabled = false;
        OnRelease?.Invoke(Holder);
        // OnDrop?.Invoke();

        Holder = null;
        // FollowPosition = null;

        // rb.isKinematic = false;
    }

    public virtual void Extract()
    {
        // Note: move to client RPC
        transform.GetComponent<Renderer>().enabled = true;
        transform.GetComponent<Collider>().enabled = true;
        FollowPosition = null;
        // DropItemServerRpc();
    }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void PickItemServerRpc(NetworkObjectReference holdPositionRef)
    {
        PickItemClientRpc(holdPositionRef);
    }

    [ClientRpc]
    protected void PickItemClientRpc(NetworkObjectReference holdPositionRef)
    {
        if (!holdPositionRef.TryGet(out NetworkObject networkObject)) return;

        // Old Holder
        if (Holder != null) OnRelease?.Invoke(Holder);

        // New Holder
        Holder = networkObject.GetComponent<IHolder>();
        FollowPosition = Holder.HoldTransform;
        rb.isKinematic = true;
        OnHold?.Invoke(Holder);
        // OnPick?.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DropItemServerRpc()
    {
        DropItemClientRpc();
    }

    [ClientRpc]
    protected void DropItemClientRpc()
    {
        OnRelease?.Invoke(Holder);
        OnDrop?.Invoke();

        Holder = null;
        FollowPosition = null;

        rb.isKinematic = false;
    }

    #endregion
}
