using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKeeper : NetworkBehaviour, IObjectKeeper
{
    private readonly Dictionary<int, PickableObject> _inventory = new();
    public Dictionary<int, PickableObject> Inventory { get => _inventory; }
    private Transform _extractPosition;
    public Vector3 ExtractPosition { get => _extractPosition.position; }

    void Start()
    {
        _extractPosition = transform.Find("Hand").transform;
    }

    public void StoreAction(IHolder holder, int inventoryKey)
    {
        PickableObject storeObject = holder.PickedObject;

        if (storeObject == null) return;
        if (_inventory.ContainsKey(inventoryKey)) return;

        _inventory.Add(inventoryKey, storeObject);
        storeObject.Drop(NetworkObject);
        storeObject.Store(NetworkObject);
    }

    public void ExtractAction(IHolder holder, int inventoryKey)
    {
        if (holder.PickedObject != null) return;
        if (!_inventory.Remove(inventoryKey, out PickableObject extractedObject)) return;

        extractedObject.Extract();
        extractedObject.Pick(NetworkObject);
    }
}
