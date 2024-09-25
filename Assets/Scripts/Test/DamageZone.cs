using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Creature victim = other.transform.parent.GetComponent<Player>();

        if (victim == null) return;

        Debug.LogWarning("Damaging Creature");
        victim.Damage(100);
    }
}
