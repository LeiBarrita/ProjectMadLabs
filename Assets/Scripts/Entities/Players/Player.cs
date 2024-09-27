using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : Creature, IHolder, IObjectKeeper
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;
    public event Action OnPlayerDestroy;

    // IHolder Properties
    private Transform _holdTranform;
    public Transform HoldTransform { get => _holdTranform; }
    private Transform _holderTranform;
    public Transform HolderTransform { get => _holderTranform; }
    private PickableObject _pickedObject;
    public PickableObject PickedObject { get => _pickedObject; }

    // IObjectKeeper Properties
    public Dictionary<string, PickableObject> _inventory = new();
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

    #region IHolder

    public void PickObject(PickableObject pickableObject)
    {
        if (_pickedObject != null) return;

        _pickedObject = pickableObject;
        _pickedObject.OnRelease += ReleaseObject;

        pickableObject.Pick(NetworkObject);
        Debug.Log("Object Picked");
    }

    public void DropObject()
    {
        if (_pickedObject == null) return;

        _pickedObject.Drop();
        _pickedObject = null;
    }

    public void ReleaseObject(IHolder holder)
    {
        _pickedObject = null;
    }

    #endregion

    #region IObjectKeeper

    public void StorePickedObject(string key)
    {
        if (_pickedObject == null) return;

        _inventory.Add(key, _pickedObject);
        ShrinkObject(_pickedObject);
        ReleaseObject(this);
    }

    public void UnloadObject(string key)
    {
        if (_pickedObject != null) return;
        if (!_inventory.Remove(key, out PickableObject unloadObject)) return;

        PickObject(unloadObject);
        GrowObject(unloadObject);
    }

    private void ShrinkObject(PickableObject objectToShrink)
    {
        objectToShrink.transform.GetComponent<Renderer>().enabled = false;
        objectToShrink.transform.GetComponent<Collider>().enabled = false;
    }

    private void GrowObject(PickableObject objectToGrow)
    {
        objectToGrow.transform.GetComponent<Renderer>().enabled = true;
        objectToGrow.transform.GetComponent<Collider>().enabled = true;
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
