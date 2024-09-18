using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GravityShifter : PickableObject
{

    private void Start()
    {
        OnHold += RemoveGravity;
        OnRelease += ReturnGravity;
    }

    public void RemoveGravity(IHolder holder)
    {
        Debug.LogWarning("GravityShifter -> OnPick: Holder:" + Holder.HolderTransform);

        // Gravedad se invierta
        holder.HolderTransform.GetComponent<Rigidbody>().useGravity = false;
    }

    public void ReturnGravity(IHolder holder)
    {
        Debug.LogWarning("GravityShifter -> OnDrop: Holder:" + Holder.HolderTransform);
        holder.HolderTransform.GetComponent<Rigidbody>().useGravity = true;

        // Debug.LogWarning("Returning gravity on " + Holder.OwnerClientId);
    }
}
