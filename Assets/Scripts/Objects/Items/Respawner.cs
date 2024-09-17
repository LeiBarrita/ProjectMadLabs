using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Respawner : PickableObject
{
    private void Start()
    {
        OnHold += Yeet10;

        OnDrop += Yeet100;
    }

    private void Yeet10(IHolder holder)
    {
        holder.HolderTransform.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    private void Yeet100(IHolder holder)
    {
        holder.HolderTransform.GetComponent<Rigidbody>().AddForce(Vector3.up * 100, ForceMode.Impulse);
    }

    // public override void OnDrop(IHolder holder)
    // {
    //     holder.HolderTransform.GetComponent<Rigidbody>().AddForce(Vector3.up * 100, ForceMode.Impulse);
    //     // base.OnDrop(holder);
    // }

    // public override void OnPick(IHolder holder)
    // {
    //     // base.OnPick(holder);
    //     holder.HolderTransform.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
    // }
}
