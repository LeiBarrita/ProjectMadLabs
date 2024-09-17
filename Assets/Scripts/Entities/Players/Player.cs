using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : Creature, IHolder
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;

    private Transform _holdTranform;
    public Transform HoldTransform
    {
        get => _holdTranform;
    }

    private Transform _holderTranform;
    public Transform HolderTransform
    {
        get => _holderTranform;
    }

    public event Action OnPlayerDestroy;

    void Start()
    {
        onPlayerSpawns.Raise(this, null);

        _holderTranform = transform;
        _holdTranform = transform.Find("Hand").transform;
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

    // public Transform GetTranform()
    // {
    //     Transform holdPositon = transform.Find("Hand").transform;
    //     return holdPositon;
    // }
}
