using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;

    public event Action OnPlayerDestroy;

    void Start()
    {
        onPlayerSpawns.Raise(this, null);
    }

    void Awake()
    {
        gameObject.AddComponent<PlayerController>();
    }

    void Update()
    {

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        OnPlayerDestroy?.Invoke();
        Debug.LogWarning("Player Destroyed");
    }
}
