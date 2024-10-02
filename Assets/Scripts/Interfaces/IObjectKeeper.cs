using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectKeeper
{
    Dictionary<int, PickableObject> Inventory { get; }
    Vector3 ExtractPosition { get; }
}
