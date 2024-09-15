using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Item : NetworkBehaviour
{
    public bool CanShrink;
    protected Player Holder;
    protected Transform HoldPosition;

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

    public virtual void Drop()
    {
        DropItemServerRpc();
    }

    public virtual void OnDrop()
    {
        Debug.LogWarning("Dropped by " + Holder.OwnerClientId);
        Holder = null;
        HoldPosition = null;
    }

    public virtual void Pick(NetworkObjectReference playerRef)
    {
        PickItemServerRpc(playerRef);
    }

    public virtual void OnPick(Player player, IHolder holder)
    {
        // DropItemServerRpc();
        Transform holdPosition = holder.GetHoldPosition();
        HoldPosition = holdPosition;
        Holder = player;
    }

    public virtual void OnThrow(Vector3 direction, float force)
    {
        Debug.Log("Throw!");
    }

    public virtual void OnHold() { }
    public virtual void OnActivate() { }
    public virtual void OnCharge() { }
    public virtual void OnRelease() { }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    private void PickItemServerRpc(NetworkObjectReference holdPositionRef)
    {
        PickItemClientRpc(holdPositionRef);
    }

    [ClientRpc]
    private void PickItemClientRpc(NetworkObjectReference holdPositionRef)
    {
        if (holdPositionRef.TryGet(out NetworkObject networkObject))
        {
            Player player = networkObject.GetComponent<Player>();
            IHolder holder = networkObject.GetComponent<IHolder>();

            OnPick(player, holder);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropItemServerRpc()
    {
        DropItemClientRpc();
    }

    [ClientRpc]
    private void DropItemClientRpc()
    {
        OnDrop();
    }

    #endregion
}
