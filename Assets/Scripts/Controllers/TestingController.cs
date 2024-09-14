using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;

public class TestingController : NetworkBehaviour
{
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.N))
        {
            // Debug.Log("N Press");
            NetworkItemsManager.Instance.SpawnNetworkItem();
        }
    }
}
