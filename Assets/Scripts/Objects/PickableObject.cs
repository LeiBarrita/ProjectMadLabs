using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
    // public event Action OnPick; // Action trigger when pick with no previous holder
    public event Action<IHolder> OnHold; // Action trigger always when pick
    public event Action<IHolder> OnDrop; // Action trigger when drop on the floor
    // public event Action<IHolder> OnRelease; // Action trigger when previous holder stops holding

    public IHolder Holder;
    // public bool CanShrink;

    private Transform FollowPosition;
    private Rigidbody rb;

    // Temporal
    protected virtual void Awake()
    {
        // OnDrop += RemoveCurrentHolder;

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

    public virtual void Pick(NetworkObjectReference holderRef)
    {
        PickItemServerRpc(holderRef);
    }

    public virtual void Drop()
    {
        DropItemServerRpc();
    }

    protected virtual void RemoveCurrentHolder(IHolder holder)
    {
        Debug.Log("PickableObject -> RemoveCurrentHolder -> Start: { Holder: " + (Holder != null) + ", holder: " + (holder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");

        Holder = null;
        FollowPosition = null;
        rb.isKinematic = false;

        Debug.Log("PickableObject -> RemoveCurrentHolder -> End: { Holder: " + (Holder != null) + ", holder: " + (holder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");
    }

    protected virtual void SetNewHolder(IHolder holder)
    {
        Debug.Log("PickableObject -> SetNewHolder -> Start: { Holder: " + (Holder != null) + ", holder: " + (holder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");

        Holder = holder;
        FollowPosition = holder.HoldTransform;
        rb.isKinematic = true;

        Debug.Log("PickableObject -> SetNewHolder -> End: { Holder: " + (Holder != null) + ", holder: " + (holder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");
    }

    // public virtual void Store()
    // {
    //     StoreItemServerRpc();
    // }

    // public virtual void Extract()
    // {
    //     ExtractItemServerRpc();
    // }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void PickItemServerRpc(NetworkObjectReference holderRef)
    {
        PickItemClientRpc(holderRef);
    }

    [ClientRpc]
    protected void PickItemClientRpc(NetworkObjectReference holderRef)
    {
        Debug.Log("PickableObject -> PickItemClientRpc -> Start: { Holder: " + (Holder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");

        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IHolder newHolder)) return;

        // Old Holder
        if (Holder != null) OnDrop?.Invoke(Holder);

        SetNewHolder(newHolder);
        OnHold?.Invoke(newHolder);

        Debug.Log("PickableObject -> PickItemClientRpc -> End: { Holder: " + (Holder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DropItemServerRpc()
    {
        DropItemClientRpc();
    }

    [ClientRpc]
    protected void DropItemClientRpc()
    {
        // if (Holder == null) return;
        OnDrop?.Invoke(Holder);
        RemoveCurrentHolder(Holder);
    }

    // [ServerRpc(RequireOwnership = false)]
    // protected void StoreItemServerRpc()
    // {
    //     StoreItemClientRpc();
    // }

    // [ClientRpc]
    // protected void StoreItemClientRpc()
    // {
    //     transform.GetComponent<Renderer>().enabled = false;
    //     transform.GetComponent<Collider>().enabled = false;
    //     OnRelease?.Invoke(Holder);
    //     Holder = null;
    // }

    // [ServerRpc(RequireOwnership = false)]
    // protected void ExtractItemServerRpc()
    // {
    //     ExtractItemClientRpc();
    // }

    // [ClientRpc]
    // protected void ExtractItemClientRpc()
    // {
    //     transform.GetComponent<Renderer>().enabled = true;
    //     transform.GetComponent<Collider>().enabled = true;
    //     FollowPosition = null;
    // }

    #endregion
}
