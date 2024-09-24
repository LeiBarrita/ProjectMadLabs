using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ProtoypeNo1 : PrototypeObject
{
    protected override void Start()
    {
        base.Start();

        OnActivationDown += Teleport;
        OnFailure += Explode;
    }

    private void Teleport(IHolder holder)
    {
        // holder.HolderTransform.position += Vector3.up * 10;
    }

    private async void Explode()
    {
        Debug.LogWarning("Failure Trigger");

        await Task.Delay(1000);
        // Destroy(this);
        // transform.GetComponent<NetworkObject>().Despawn();
        NetworkObject.Despawn();
    }
}
