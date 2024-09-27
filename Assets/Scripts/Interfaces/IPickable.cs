using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deprecated, use PickableObject instead
public interface IPickable
{
    bool CanShrink { get; }
    bool CanActivate { get; }
    void Pick(Player player, Transform pos);
    void Throw(Vector3 direction, float force);
    void Drop();
    // void Activate
}
