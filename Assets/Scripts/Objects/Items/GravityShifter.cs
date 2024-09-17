using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GravityShifter : Item
{
    // public override void OnPick()
    // {
    //     base.OnPick();
    //     Debug.LogWarning("GravityShifter -> OnPick: Holder:" + Holder.HolderTransform);

    //     // Gravedad se invierta
    //     transform.GetComponent<Rigidbody>().isKinematic = true;
    //     Holder.HolderTransform.GetComponent<Rigidbody>().useGravity = false;
    // }

    // public override void OnDrop()
    // {
    //     Debug.LogWarning("GravityShifter -> OnDrop: Holder:" + Holder.HolderTransform);
    //     transform.GetComponent<Rigidbody>().isKinematic = false;
    //     Holder.HolderTransform.GetComponent<Rigidbody>().useGravity = true;

    //     // Debug.LogWarning("Returning gravity on " + Holder.OwnerClientId);
    //     base.OnDrop();
    // }
}
