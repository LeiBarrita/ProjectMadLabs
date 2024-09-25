using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Creature : NetworkBehaviour
{
    private NetworkVariable<int> _lifePoints = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public event Action<Creature> OnDeath;
    public int MaxLifePoints = 100;
    public int LifePoints
    {
        get { return _lifePoints.Value; }
        set { _lifePoints.Value = Math.Clamp(value, 0, MaxLifePoints); }
    }

    protected virtual void Start()
    {
        LifePoints = MaxLifePoints;
    }

    // public CreatureState State;

    // public enum CreatureState
    // {
    //     Death,
    //     Alive,
    //     Unknow,
    // }

    public virtual void Damage(int damage)
    {
        // Debug.Log("Damage received: " + damage);
        LifePoints -= damage;

        if (LifePoints <= 0) TriggerDeath();
    }

    protected virtual void TriggerDeath()
    {
        TriggerDeathServerRpc();
    }

    #region RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void TriggerDeathServerRpc()
    {
        TriggerDeathClientRpc();
    }

    [ClientRpc]
    protected void TriggerDeathClientRpc()
    {
        // Debug.Log("Creature Dead");
        OnDeath?.Invoke(this);
    }

    #endregion
}
