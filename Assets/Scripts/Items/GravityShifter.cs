using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GravityShifter : NetworkBehaviour
{
    public void Activate()
    {
        Debug.Log("Gravity Shifter Picked!");
    }
}
