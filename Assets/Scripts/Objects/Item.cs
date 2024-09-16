using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Item : PickableObject
{
    // protected Player Holder;

    public virtual void OnDrop()
    {
        // Debug.LogWarning("Dropped by " + Holder.OwnerClientId);
        // Holder = null;
        // HoldPosition = null;
    }

    public virtual void OnPick(Player player, IHolder holder)
    {
        // Transform holdPosition = holder.GetTranform();
        // Holder = player;
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
