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
        // Debug.Log("Holding by " + Holder.OwnerClientId);
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

    public virtual void OnPick(Player player, Transform holdPos)
    {
        Holder = player;
        HoldPosition = holdPos;
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
    private void SetHolderServerRpc()
    {

    }

    [ClientRpc]
    private void SetHolderClientRpc(NetworkObjectReference playerRefence, NetworkObjectReference holdPosReference)
    {
        if (playerRefence.TryGet(out NetworkObject networkObject))
        {
            Player player = networkObject.GetComponent<Player>();
            Holder = player;
            // HoldPosition = holdPos;
        }
    }
}
