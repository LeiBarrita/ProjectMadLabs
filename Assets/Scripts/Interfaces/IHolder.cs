using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHolder
{
    Transform HoldTransform { get; }
    Transform HolderTransform { get; }
    PickableObject PickedObject { get; }
    public void PickObject(PickableObject pickableObject);
    public void TryReleaseObject(IHolder holder);
    public void DropObject();
}
