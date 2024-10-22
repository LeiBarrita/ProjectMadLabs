using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IFuelHolder
{
    Transform FuelHoldSpace { get; }
    NetworkObjectReference FuelHolderRef { get; }
    public void PickFuel(Fuel fuel);
    public void DropFuel();
}