using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
    public event Action<IHolder> OnHold;
    public event Action<IHolder> OnDrop;

    private Transform HoldPosition;

    public IHolder Holder;
    // public bool CanShrink;

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
    private void PickItemServerRpc(NetworkObjectReference holdPositionRef)
    {
        PickItemClientRpc(holdPositionRef);
    }

    [ClientRpc]
    private void PickItemClientRpc(NetworkObjectReference holdPositionRef)
    {
        if (Holder != null) OnDrop?.Invoke(Holder);

        if (holdPositionRef.TryGet(out NetworkObject networkObject))
        {
            Holder = networkObject.GetComponent<IHolder>();
            HoldPosition = Holder.HoldTransform;
        }

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
        OnDrop?.Invoke(Holder);

        Holder = null;
        HoldPosition = null;

    }

    #endregion
}
