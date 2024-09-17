using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Item : PickableObject
{
    // public override void Pick(NetworkObjectReference playerRef)
    // {
    //     IHolder prevHolder = Holder;

    //     base.Pick(playerRef);

    //     // if (playerRef.TryGet(out NetworkObject networkObject))
    //     // {
    //     //     if (networkObject.TryGetComponent(out IHolder holder))
    //     //         OnPick(holder);
    //     // }

    //     if (prevHolder != null) OnDrop(prevHolder);
    //     OnPick(Holder);
    // }

    // public override void Drop()
    // {
    //     IHolder prevHolder = Holder;
    //     base.Drop();

    //     if (prevHolder != null) OnDrop(prevHolder);
    // }

    // public virtual void OnDrop(IHolder holder) { }
    // public virtual void OnPick(IHolder holder) { }
    // public virtual void OnPick(IHolder holder) { }
    // public virtual void OnHold() { }

    public virtual void OnActivationDown() { }
    public virtual void OnActivationStay() { }
    public virtual void OnActivationUp() { }
}
