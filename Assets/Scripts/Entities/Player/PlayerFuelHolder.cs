using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerFuelHolder : NetworkBehaviour, IFuelHolder
{
    private readonly Stack<Fuel> _fuelInventory = new();
    public Transform[] FuelSpaces;
    public Transform FuelHoldSpace { get => GetFreeFuelStorage(); }
    public NetworkObjectReference FuelHolderRef { get => NetworkObject; }
    public Stack<Fuel> FuelInventory { get => _fuelInventory; }
    public int FuelHoldingCount { get => _fuelInventory.Count; }

    public void PickFuel(Fuel fuel)
    {
        if (FuelHoldingCount >= FuelSpaces.Length) return;
        _fuelInventory.Push(fuel);
    }

    public void DropFuel()
    {
        if (FuelHoldingCount < 1) return;
        _fuelInventory.Pop();
    }

    private Transform GetFreeFuelStorage()
    {
        // Debug.Log("GetFreeFuelStorage -> FuelHoldingCount:" + FuelHoldingCount);
        // Debug.Log("GetFreeFuelStorage -> FuelSpaces" + FuelSpaces.Length);
        if (FuelHoldingCount >= FuelSpaces.Length) return null;
        return FuelSpaces[FuelHoldingCount];
    }
}
