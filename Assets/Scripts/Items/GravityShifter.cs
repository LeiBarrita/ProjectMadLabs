using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GravityShifter : Item
{
    public override void OnPick(Player player, IHolder holder)
    {
        base.OnPick(player, holder);
        Debug.LogWarning("Inverting gravity on " + Holder.OwnerClientId);

        // Gravedad se invierta
        transform.GetComponent<Rigidbody>().isKinematic = true;
        Holder.GetComponent<Rigidbody>().useGravity = false;
    }

    public override void OnDrop()
    {
        transform.GetComponent<Rigidbody>().isKinematic = false;
        Holder.GetComponent<Rigidbody>().useGravity = true;

        Debug.LogWarning("Returning gravity on " + Holder.OwnerClientId);
        base.OnDrop();
    }
}
