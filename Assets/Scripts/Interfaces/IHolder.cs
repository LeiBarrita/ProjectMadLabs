using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IHolder
{
    Transform HoldTransform { get; }
    Transform HolderTransform { get; }
    PickableObject PickedObject { get; }
    NetworkObjectReference HolderRef { get; }
    public void PickObject(PickableObject pickableObject);
    public void DropObject();
}
