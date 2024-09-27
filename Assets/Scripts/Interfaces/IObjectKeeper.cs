using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectKeeper
{
    Dictionary<string, PickableObject> Inventory { get; }
    public void StorePickedObject(string key);
    public void UnloadObject(string key);
}
