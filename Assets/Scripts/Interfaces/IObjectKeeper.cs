using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectKeeper
{
    Dictionary<int, PickableObject> Inventory { get; }

    // bool TryStorePickedObject(string key);
    // bool TryExtractObject(string key);
}
