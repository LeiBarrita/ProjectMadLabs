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

    private void Explode()
    {
        // DespawnObjectServerRpc();
    }

    // public async Task DestroyNetworkObject()
    // {
    //     await Task.Delay(1000);
    //     NetworkObject.Despawn();
    // }

    // #region RPCs

    // [ServerRpc(RequireOwnership = false)]
    // protected void DespawnObjectServerRpc()
    // {
    //     DespawnObjectClientRpc();
    // }

    // [ClientRpc]
    // protected void DespawnObjectClientRpc()
    // {
    //     Debug.LogWarning("Failure Trigger");
    //     _ = DestroyNetworkObject();
    // }

    // #endregion
}
