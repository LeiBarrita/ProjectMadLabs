using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Creature : NetworkBehaviour
{
    [SerializeField] private int baseLifePoints;

    protected int lifePoints;

    public event Action OnPlayerDeath;
    public CreatureState State;

    public enum CreatureState
    {
        Death,
        Alive,
        Unknow,
    }

    public virtual void Damage(int damage)
    {
        Debug.Log("Damage received: " + damage);

        lifePoints -= damage;

        if (lifePoints <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        // Added death effects
        OnPlayerDeath?.Invoke();

        Debug.Log("Dies :skull:");
    }
}
