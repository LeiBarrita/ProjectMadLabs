using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Player : Creature, IHolder, IObjectKeeper
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;
    public event Action OnPlayerDestroy;

    // private readonly int _inventorySize = 4;

    // IHolder Properties
    private Transform _holdTranform;
    public Transform HoldTransform { get => _holdTranform; }
    private Transform _holderTranform;
    public Transform HolderTransform { get => _holderTranform; }
    private PickableObject _pickedObject;
    public PickableObject PickedObject { get => _pickedObject; }
    public NetworkObjectReference HolderRef { get => NetworkObject; }

    // IObjectKeeper Properties
    private readonly Dictionary<string, PickableObject> _inventory = new();
    public Dictionary<string, PickableObject> Inventory { get => _inventory; }

    override protected void Start()
    {
        base.Start();
        onPlayerSpawns.Raise(this, null);
        OnDeath += SimulateDeath;

        _holderTranform = transform;
        _holdTranform = transform.Find("Hand").transform;
    }

    // public override void Damage(int damage)
    // {
    //     if (LifePoints > 10 && damage > 10)
    //         LifePoints = 1;
    //     else
    //     {
    //         base.Damage(damage);
    //     }
    // }

    public override void OnDestroy()
    {
        base.OnDestroy();
        OnPlayerDestroy?.Invoke();
    }

    public void PickAction(PickableObject pickableObject)
    {
        pickableObject.Pick(NetworkObject);
    }

    public void DropAction()
    {
        _pickedObject.Drop(NetworkObject);
    }

    #region IHolder

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

    #endregion

    #region IObjectKeeper

    public bool TryStorePickedObject(string key)
    {
        // if (_pickedObject == null) return false;
        // if (_inventory.ContainsKey(key)) return false;
        // _inventory.Add(key, _pickedObject);

        // // RemovePickedObject(this);
        // _pickedObject.Store();
        return true;
    }

    public bool TryExtractObject(string key)
    {
        // if (_pickedObject != null) return false;
        // if (!_inventory.Remove(key, out PickableObject extractableObject)) return false;

        // extractableObject.Extract();
        // PickObject(extractableObject);
        return true;
    }

    #endregion

    #region  Death Simulation
    private void SimulateDeath(Creature creature)
    {
        DelayedRespawn();
    }

    private async void DelayedRespawn()
    {
        PlayerController pc = transform.GetComponent<PlayerController>();
        PickController pickc = transform.GetComponent<PickController>();

        pc.enabled = false;
        pickc.enabled = false;

        await Task.Delay(3000);

        transform.position = Vector3.zero;
        pickc.enabled = true;
        pc.enabled = true;
        LifePoints = MaxLifePoints;
    }

    #endregion
}
