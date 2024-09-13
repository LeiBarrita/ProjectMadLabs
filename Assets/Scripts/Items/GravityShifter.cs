using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GravityShifter : Item
{
    public override void OnPick(Player player, Transform pos)
    {
        base.OnPick(player, pos);

        // Gravedad se invierta
        Holder.GetComponent<Rigidbody>().useGravity = false;
    }
}
