using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerHolder))]
public class Player : Creature, IObjectKeeper, IFuelHolder
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;
    public GameEvent onPlayerDies;
    public event Action OnPlayerDestroy;

    public PlayerHolder Holder;

    // IObjectKeeper Properties
    private readonly Dictionary<int, PickableObject> _inventory = new();
    public Dictionary<int, PickableObject> Inventory { get => _inventory; }
    public Vector3 ExtractPosition { get => Holder.HolderTransform.position; }

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

        Holder = GetComponent<PlayerHolder>();
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

    #region IObjectKeeper

    public void StoreAction(int inventoryKey)
    {
        PickableObject storeObject = Holder.PickedObject;

        if (storeObject == null) return;
        if (_inventory.ContainsKey(inventoryKey)) return;

        _inventory.Add(inventoryKey, storeObject);
        storeObject.Drop(NetworkObject);
        storeObject.Store(NetworkObject);
    }

    public void ExtractAction(int inventoryKey)
    {
        if (Holder.PickedObject != null) return;
        if (!_inventory.Remove(inventoryKey, out PickableObject extractedObject)) return;

        extractedObject.Extract();
        extractedObject.Pick(NetworkObject);
    }

    #endregion

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
