using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Respawner : ActivableObject
{
    protected override void Start()
    {
        base.Start();

        OnHold += Yeet10;
        OnDrop += Yeet100;

        OnActivationDown += Ascend;
        OnActivationUp += ResetPosition;

        Debug.Log("Respawner Created");
    }


    private void Yeet10(IHolder holder)
    {
        // holder.HolderTransform.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
        holder.HolderTransform.position = new Vector3(0, 5, 0);
    }

    private void Yeet100(IHolder holder)
    {
        holder.HolderTransform.GetComponent<Rigidbody>().AddForce(Vector3.up * 30, ForceMode.Impulse);
    }

    private void Ascend(IHolder holder)
    {
        holder.HolderTransform.position += Vector3.up * 0.5f;
    }

    private void ResetPosition(IHolder holder)
    {
        holder.HolderTransform.position = Vector3.zero;
    }
}
