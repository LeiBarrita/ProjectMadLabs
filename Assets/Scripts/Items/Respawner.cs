using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Respawner : NetworkBehaviour, IPickable
{
    public void Activate()
    {
        Debug.Log("Respawner Picked!");
    }
}
