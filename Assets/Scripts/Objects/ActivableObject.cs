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
        OnDrop += ResetActivation;
    }

    public virtual void ActivateKeyDown(NetworkObjectReference holderRef)
    {
        ActivateKeyDownServerRpc(holderRef);
    }

    public virtual void ActivateKeyUp(NetworkObjectReference holderRef)
    {
        ActivateKeyUpServerRpc(holderRef);
    }

    private void ActivateOn(IHolder holder)
    {
        if (Active) return;

        Active = true;
        OnActivationDown?.Invoke(holder);
    }

    private void ActivateOff(IHolder holder)
    {
        if (!Active) return;

        OnActivationUp?.Invoke(holder);
        Active = false;
    }

    private void ResetActivation(IHolder holder)
    {
        // Debug.Log("ActivableObject -> ResetActivation -> Start: { Active: " + Active + ", holder: " + (holder != null) + " }");

        if (!ErrorHandler.ValueExists(holder, "ActivableObject", "ResetActivation", "holder")) return;

        ActivateOff(holder);

        // Debug.Log("ActivableObject -> ResetActivation -> End: { Active: " + Active + ", holder: " + (holder != null) + " }");
    }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void ActivateKeyDownServerRpc(NetworkObjectReference holderRef)
    {
        ActivateKeyDownClientRpc(holderRef);
    }

    [ClientRpc]
    protected void ActivateKeyDownClientRpc(NetworkObjectReference holderRef)
    {
        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        IHolder holder = networkObject.transform.GetComponent<IHolder>();
        if (!ErrorHandler.ValueExists(holder, "ActivableObject", "ActivateKeyDownClientRpc", "holder")) return;

        ActivateOn(holder);
    }

    [ServerRpc(RequireOwnership = false)]
    protected void ActivateKeyUpServerRpc(NetworkObjectReference holderRef)
    {
        ActivateKeyUpClientRpc(holderRef);
    }

    [ClientRpc]
    protected void ActivateKeyUpClientRpc(NetworkObjectReference holderRef)
    {
        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        IHolder holder = networkObject.transform.GetComponent<IHolder>();
        if (!ErrorHandler.ValueExists(holder, "ActivableObject", "ActivateKeyUpClientRpc", "holder")) return;

        // Debug.Log("ActivableObject -> ActivateKeyUpClientRpc -> Start: { Active: " + Active + ", Holder: " + (Holder != null) + ", holder: " + (holder != null) + " }");

        ActivateOff(holder);

        // Debug.Log("ActivableObject -> ActivateKeyUpClientRpc -> End: { Active: " + Active + ", Holder: " + (Holder != null) + ", holder: " + (holder != null) + " }");
    }

    #endregion
}
