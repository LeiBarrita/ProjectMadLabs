using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkRpcManager : NetworkBehaviour
{
    public static NetworkRpcManager Instance { get; private set; }

    // Separate Later
    public List<GameObject> spawnableObjects;

    private void Awake()
    {
        // if (IsClient)
        // {
        //     Debug.LogWarning("Evil Clone Destroyed!");
        //     Destroy(gameObject);
        //     return;
        // }

        Instance = this;
        // DontDestroyOnLoad(this);
    }

    public void SpawnNetworkItem(int objectIndex)
    {
        SpawnNetworkItemServerRpc(objectIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnNetworkItemServerRpc(int objectIndex)
    {
        GameObject networkItemTrans = Instantiate(spawnableObjects[objectIndex], Vector3.zero, Quaternion.identity);

        NetworkObject networkObjectItem = networkItemTrans.GetComponent<NetworkObject>();
        networkObjectItem.Spawn(true);

        // Item item = networkItemTrans.GetComponent<Item>();
        // networkItem.SetParent();
    }
}
