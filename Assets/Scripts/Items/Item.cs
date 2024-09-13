using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Item : NetworkBehaviour
{
    public bool CanShrink;
    protected Player Holder;

    public virtual void OnDrop()
    {
        Debug.LogWarning("Item drop");
        Holder = null;
        transform.parent = null;
    }

    public virtual void OnPick(Player player, Transform pos)
    {
        Holder = player;
        transform.parent = player.GetComponent<Transform>();
        transform.localPosition = pos.localPosition;
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
