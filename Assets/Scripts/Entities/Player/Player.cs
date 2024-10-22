using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerHolder))]
[RequireComponent(typeof(PlayerKeeper))]
public class Player : Creature, IFuelHolder
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;
    public GameEvent onPlayerDies;
    public event Action OnPlayerDestroy;

    [Header("Interactions")]
    public PlayerHolder Holder;
    public PlayerKeeper Keeper;

    // IFuelHolder Properties
    [Header("Fuel")]
    [SerializeField] private Transform[] _fuelSpaces;
    private Stack<Fuel> _fuelInventory = new();
    public int FuelHoldingCount { get => _fuelInventory.Count; }
    public Transform FuelHoldSpace { get => GetFreeFuelStorage(); }
    public NetworkObjectReference FuelHolderRef { get => NetworkObject; }

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
        if (FuelHoldingCount >= _fuelSpaces.Length) return;
        if (fuel.FuelHolder != null) return;

        fuel.Pick(NetworkObject);
    }

    public void DropFuelAction()
    {
        if (FuelHoldingCount < 1) return;

        Fuel droppedFuel = _fuelInventory.Peek();
        droppedFuel.Drop(NetworkObject);
    }

    #region IFuelHolder

    public void PickFuel(Fuel fuel)
    {
        if (FuelHoldingCount >= _fuelSpaces.Length) return;
        _fuelInventory.Push(fuel);
    }

    public void DropFuel()
    {
        if (FuelHoldingCount < 1) return;
        _fuelInventory.Pop();
    }

    private Transform GetFreeFuelStorage()
    {
        Debug.Log("GetFreeFuelStorage -> FuelHoldingCount:" + FuelHoldingCount);
        Debug.Log("GetFreeFuelStorage -> FuelSpaces" + _fuelSpaces.Length);
        if (FuelHoldingCount >= _fuelSpaces.Length) return null;
        return _fuelSpaces[FuelHoldingCount];
    }

    #endregion

    #region  Death Simulation
    private void SimulateDeath(Creature creature)
    {
        onPlayerDies.Raise(null, OwnerClientId);
        NetworkObject.Despawn();
    }

    #endregion
}
