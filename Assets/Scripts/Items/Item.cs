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

    public virtual void OnDrop()
    {
        Debug.LogWarning("Dropped by " + Holder.OwnerClientId);
        Holder = null;
        HoldPosition = null;
    }

    // public virtual void OnPick(Player player, Transform holdPos)
    public virtual void OnPick(NetworkObjectReference holdPositionRef)
    {
        SetHolderServerRpc(holdPositionRef);
        Debug.LogWarning("Picked by " + Holder.OwnerClientId);
    }

    public virtual void OnThrow(Vector3 direction, float force)
    {
        Debug.Log("Throw!");
    }

    public virtual void OnHold() { }
    public virtual void OnActivate() { }
    public virtual void OnCharge() { }
    public virtual void OnRelease() { }

    // RPCs

    [ServerRpc(RequireOwnership = false)]
    private void SetHolderServerRpc(NetworkObjectReference holdPositionRef)
    {
        SetHolderClientRpc(holdPositionRef);
    }

    [ClientRpc]
    private void SetHolderClientRpc(NetworkObjectReference holdPositionRef)
    {
        if (holdPositionRef.TryGet(out NetworkObject networkObject))
        {
            // Player player = networkObject.GetComponentInParent<Player>();
            // Transform holdPosition = networkObject.transform;
            Player player = networkObject.GetComponent<Player>();
            IHolder holder = networkObject.GetComponent<IHolder>();
            Transform holdPosition = holder.GetHoldPosition();

            HoldPosition = holdPosition;
            Holder = player;
        }
    }
}
