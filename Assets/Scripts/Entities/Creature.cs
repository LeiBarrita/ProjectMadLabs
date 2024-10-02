using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

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

    // public CreatureStatus Status = CreatureStatus.Alive;
    // public enum CreatureStatus
    // {
    //     Death,
    //     Alive,
    //     Unknow,
    // }

    protected virtual void Start()
    {
        if (IsOwner) LifePoints = MaxLifePoints;
    }

    public virtual void Damage(int damage)
    {
        if (LifePoints <= 0) return;

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
        // Status = CreatureStatus.Death;

        if (this is IHolder holder) holder.PickedObject?.Drop(NetworkObject);

        OnDeath?.Invoke(this);

        // NetworkObject.Despawn();
    }

    #endregion
}
