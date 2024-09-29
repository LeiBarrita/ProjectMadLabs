using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityShifter : PickableObject
{

    private void Start()
    {
        OnHold += RemoveGravity;
        OnDrop += ReturnGravity;
    }

    public void RemoveGravity(IHolder holder)
    {
        holder.HolderTransform.GetComponent<Rigidbody>().useGravity = false;
    }

    public void ReturnGravity(IHolder holder)
    {
        holder.HolderTransform.GetComponent<Rigidbody>().useGravity = true;
    }
}
