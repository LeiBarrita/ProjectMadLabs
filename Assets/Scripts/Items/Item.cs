using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Item : NetworkBehaviour
{
    public bool CanShrink;
    protected Player Holder;
    protected Transform HoldPosition;

    private void Update()
    {
        if (HoldPosition != null)
        {
            Debug.Log("Holding by " + Holder.OwnerClientId);
            // Vector3 follow = HoldPosition.position;
            transform.SetPositionAndRotation(
                HoldPosition.position,
                Quaternion.Euler(0, -Holder.transform.rotation.y, 0)
            );
        }
    }

    public virtual void OnDrop()
    {
        Debug.LogWarning("Dropped by " + Holder.OwnerClientId);
        Holder = null;
        // transform.parent = null;
        HoldPosition = null;
    }

    public virtual void OnPick(Player player, Transform holdPos)
    {
        Holder = player;
        HoldPosition = holdPos;
        Debug.LogWarning("Picked by " + Holder.OwnerClientId);
        // transform.parent = player.GetComponent<Transform>();
        // transform.localPosition = pos.localPosition;
        // transform.position = pos.position;
    }

    public virtual void OnThrow(Vector3 direction, float force)
    {
        Debug.Log("Throw!");
    }

    public virtual void OnHold() { }
    public virtual void OnActivate() { }
    public virtual void OnCharge() { }
    public virtual void OnRelease() { }
}
