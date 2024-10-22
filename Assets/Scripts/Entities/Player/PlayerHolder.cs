using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHolder : NetworkBehaviour, IHolder
{
    private Transform _holdTranform;
    public Transform HoldTransform { get => _holdTranform; }
    private Transform _holderTranform;
    public Transform HolderTransform { get => _holderTranform; }
    private PickableObject _pickedObject;
    public PickableObject PickedObject { get => _pickedObject; }
    public NetworkObjectReference HolderRef { get => NetworkObject; }

    protected void Start()
    {
        _holderTranform = transform;
        _holdTranform = transform.Find("Hand").transform;
    }

    public void PickObject(PickableObject pickableObject)
    {
        if (_pickedObject != null) return;

        _pickedObject = pickableObject;
    }

    public void DropObject()
    {
        if (!ErrorHandler.ValueExists(_pickedObject, "Player", "DropObject", "_pickedObject")) return;

        _pickedObject = null;
    }
}
