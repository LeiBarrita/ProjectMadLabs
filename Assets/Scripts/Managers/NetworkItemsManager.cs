using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkItemsManager : NetworkBehaviour
{
    public static NetworkItemsManager Instance { get; private set; }

    // temporal prefab
    public GameObject prefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    // public void SpawnNetworkItem(NetworkItem networkItem)
    public void SpawnNetworkItem()
    {
        SpawnNetworkItemServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnNetworkItemServerRpc()
    {
        // Debug.Log("SpawnNetworkItem");
        GameObject networkItemTrans = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        NetworkObject networkObjectItem = networkItemTrans.GetComponent<NetworkObject>();
        networkObjectItem.Spawn(true);

        // Item item = networkItemTrans.GetComponent<Item>();
        // networkItem.SetParent();

        // return item;
    }
}
