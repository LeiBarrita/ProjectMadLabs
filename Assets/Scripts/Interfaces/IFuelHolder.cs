using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IFuelHolder
{
    Transform HoldTransform { get; }
    NetworkObjectReference HolderRef { get; }
    public void PickFuel(PickableObject pickableObject);
    public void DropFuel();
}
