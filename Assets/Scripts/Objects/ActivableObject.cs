using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class ActivableObject : PickableObject
{
    public event Action<IHolder> OnActivationDown;
    public event Action<IHolder> OnActivationUp;

    [NonSerialized]
    public bool Active = false;

    protected virtual void Start()
    {
        OnRelease += ResetActivation;
        // Debug.Log("ActivableObject Created");
    }

    public virtual void ActivateKeyDown()
    {
        ActivateKeyDownServerRpc();
    }

    public virtual void ActivateKeyUp()
    {
        ActivateKeyUpServerRpc();
    }

    private void ResetActivation(IHolder holder)
    {
        if (!Active) return;
        ActivateKeyUp();
    }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void ActivateKeyDownServerRpc()
    {
        ActivateKeyDownClientRpc();
    }

    [ClientRpc]
    protected void ActivateKeyDownClientRpc()
    {
        Active = true;
        OnActivationDown?.Invoke(Holder);
    }

    [ServerRpc(RequireOwnership = false)]
    protected void ActivateKeyUpServerRpc()
    {
        ActivateKeyUpClientRpc();
    }

    [ClientRpc]
    protected void ActivateKeyUpClientRpc()
    {
        OnActivationUp?.Invoke(Holder);
        Active = false;
    }

    #endregion
}
