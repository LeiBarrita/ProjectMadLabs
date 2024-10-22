using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerHolder))]
[RequireComponent(typeof(PlayerKeeper))]
[RequireComponent(typeof(PlayerFuelHolder))]
public class Player : Creature
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;
    public GameEvent onPlayerDies;
    public event Action OnPlayerDestroy;

    [Header("Interactions")]
    public PlayerHolder Holder;
    public PlayerKeeper Keeper;
    public PlayerFuelHolder FuelHolder;

    override protected void Start()
    {
        base.Start();
        onPlayerSpawns.Raise(this, null);
        OnDeath += SimulateDeath;
    }

    // public override void Damage(int damage)
    // {
    //     if (LifePoints > 10 && damage > 10)
    //         LifePoints = 1;
    //     else
    //     {
    //         base.Damage(damage);
    //     }
    // }

    public override void OnDestroy()
    {
        base.OnDestroy();
        OnPlayerDestroy?.Invoke();
    }

    public void PickAction(PickableObject pickableObject)
    {
        pickableObject.Pick(NetworkObject);
    }

    public void DropAction()
    {
        Holder.PickedObject.Drop(NetworkObject);
    }

    public void PickFuelAction(Fuel fuel)
    {
        if (FuelHolder.FuelHoldingCount >= FuelHolder.FuelSpaces.Length) return;
        if (fuel.FuelHolder != null) return;

        fuel.Pick(NetworkObject);
    }

    public void DropFuelAction()
    {
        if (FuelHolder.FuelHoldingCount < 1) return;

        Fuel droppedFuel = FuelHolder.FuelInventory.Peek();
        droppedFuel.Drop(NetworkObject);
    }

    #region  Death Simulation
    private void SimulateDeath(Creature creature)
    {
        onPlayerDies.Raise(null, OwnerClientId);
        NetworkObject.Despawn();
    }

    #endregion
}
