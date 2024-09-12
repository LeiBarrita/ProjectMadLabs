using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : Creature
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;

    public event Action OnPlayerDestroy;

    void Start()
    {
        onPlayerSpawns.Raise(this, null);
    }

    void Update()
    {

    }

    public override void Damage(int damage)
    {
        if (lifePoints > 10 && damage > 10)
            lifePoints = 1;
        else
        {
            base.Damage(damage);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        OnPlayerDestroy?.Invoke();
        Debug.LogWarning("Player Destroyed");
    }
}
