using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
    private Transform HoldPosition;
    public bool CanShrink;

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
        if (holdPositionRef.TryGet(out NetworkObject networkObject))
        {
            IHolder holder = networkObject.GetComponent<IHolder>();
            HoldPosition = holder.GetTranform();
        }
        PickItemClientRpc(holdPositionRef);
    }

    [ClientRpc]
    // protected virtual void PickItemClientRpc(NetworkObjectReference holdPositionRef)
    protected virtual void PickItemClientRpc(NetworkObjectReference holdPositionRef)
    {
        if (holdPositionRef.TryGet(out NetworkObject networkObject))
        {
            IHolder holder = networkObject.GetComponent<IHolder>();
            HoldPosition = holder.GetTranform();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRpc()
    {
        DropItemClientRpc();
    }

    [ClientRpc]
    protected virtual void DropItemClientRpc()
    {
        HoldPosition = null;
    }

    #endregion
}
