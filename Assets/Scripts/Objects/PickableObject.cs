using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
    public event Action OnPick; // Action trigger when pick with no previous holder
    public event Action<IHolder> OnHold; // Action trigger always when pick
    public event Action OnDrop; // Action trigger when drop on the floor
    public event Action<IHolder> OnRelease; // Action trigger when previous holder stops holding

    public IHolder Holder;
    // public bool CanShrink;

    private Transform HoldPosition;


    private void LateUpdate()
    {
        if (HoldPosition == null) return;
        FollowHoldPosition(HoldPosition);
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

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void PickItemServerRpc(NetworkObjectReference holdPositionRef)
    {
        PickItemClientRpc(holdPositionRef);
    }

    [ClientRpc]
    protected void PickItemClientRpc(NetworkObjectReference holdPositionRef)
    {
        if (Holder != null) OnRelease?.Invoke(Holder);

        if (holdPositionRef.TryGet(out NetworkObject networkObject))
        {
            Holder = networkObject.GetComponent<IHolder>();
            HoldPosition = Holder.HoldTransform;
        }

        OnPick?.Invoke();
        OnHold?.Invoke(Holder);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRpc()
    {
        DropItemClientRpc();
    }

    [ClientRpc]
    private void DropItemClientRpc()
    {
        OnRelease?.Invoke(Holder);
        OnDrop?.Invoke();

        Holder = null;
        HoldPosition = null;

    }

    #endregion
}
