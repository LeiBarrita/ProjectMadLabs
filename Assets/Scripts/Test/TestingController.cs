using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;

public class TestingController : NetworkBehaviour
{
    public List<GameObject> itemPrefabs;
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            NetworkRpcManager.Instance.SpawnNetworkItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            NetworkRpcManager.Instance.SpawnNetworkItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            NetworkRpcManager.Instance.SpawnNetworkItem(2);
        }
    }
}

