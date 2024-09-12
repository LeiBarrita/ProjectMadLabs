using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Item : NetworkBehaviour, IPickable
{
    protected Player Holder;

    private bool canShrink;
    public bool CanShrink
    {
        get { return canShrink; }
        protected set { canShrink = value; }
    }

    private bool canActivate;
    public bool CanActivate
    {
        get { return canActivate; }
        protected set { canActivate = value; }
    }

    public virtual void Drop()
    {
        Holder = null;
        transform.parent = null;
    }

    public virtual void Pick(Player player, Transform pos)
    {
        Holder = player;
        transform.parent = player.GetComponent<Transform>();
        transform.localPosition = pos.localPosition;
    }

    public virtual void Throw(Vector3 direction, float force)
    {
        Debug.Log("Throw!");
    }
}
