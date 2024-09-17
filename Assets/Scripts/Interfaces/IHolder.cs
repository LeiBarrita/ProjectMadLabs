using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHolder
{
    Transform HoldTransform { get; }
    Transform HolderTransform { get; }

    // Transform GetTranform();
    // T GetComponent<T>();
}
