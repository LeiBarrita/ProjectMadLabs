using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Player : Creature, IHolder
{
    [Header("Events")]
    public GameEvent onPlayerSpawns;

    private Transform _holdTranform;
    public Transform HoldTransform
    {
        get => _holdTranform;
    }

    private Transform _holderTranform;
    public Transform HolderTransform
    {
        get => _holderTranform;
    }

    private PickableObject _pickedObject;
    public PickableObject PickedObject
    {
        get => _pickedObject;
    }

    public event Action OnPlayerDestroy;

    override protected void Start()
    {
        base.Start();
        onPlayerSpawns.Raise(this, null);
        OnDeath += SimulateDeath;
        // OnDeath += DropOnDeath;

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

    public void PickObject(PickableObject pickableObject)
    {
        if (_pickedObject != null) return;

        _pickedObject = pickableObject;
        _pickedObject.OnRelease += ReleaseObject;

        pickableObject.Pick(NetworkObject);
    }

    // public void DropOnDeath(Creature creature)
    // {
    //     DropObject();
    // }

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

    public override void OnDestroy()
    {
        base.OnDestroy();
        OnPlayerDestroy?.Invoke();
        Debug.LogWarning("Player Destroyed");
    }

    #region  Death Simulation
    private void SimulateDeath(Creature creature)
    {
        DelayedRespawn();
    }

    private async void DelayedRespawn()
    {
        // Rigidbody rb = transform.GetComponent<Rigidbody>();
        PlayerController pc = transform.GetComponent<PlayerController>();
        PickController pickc = transform.GetComponent<PickController>();

        pc.enabled = false;
        pickc.enabled = false;
        // rb.constraints = RigidbodyConstraints.None;
        // rb.freezeRotation = false;
        // rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
        // rb.AddTorque(Vector3.one * 20, ForceMode.Impulse);
        // rb.isKinematic = false;

        await Task.Delay(3000);
        transform.position = Vector3.zero;
        // rb.constraints = RigidbodyConstraints.None;
        // rb.freezeRotation = true;
        // rb.velocity = Vector3.zero;
        pickc.enabled = true;
        pc.enabled = true;
    }

    #endregion
}
