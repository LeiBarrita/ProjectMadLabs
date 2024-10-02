using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class RespawnManager : NetworkBehaviour
{
    private readonly int RespanDelay = 3000;
    public GameObject playerPrefab;

    public void OnPlayerDies(Component sender, object data)
    {
        if (data == null || !IsHost) return;
        if (data is ulong playerId) DelayedRespawn(playerId);
    }

    private async void DelayedRespawn(ulong playerId)
    {
        await Task.Delay(RespanDelay);

        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
        playerNetworkObject.SpawnAsPlayerObject(playerId);
    }
}
