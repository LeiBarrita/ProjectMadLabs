using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public abstract class PrototypeObject : ActivableObject
{
    // private NetworkVariable<int> failureProbability = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool IsBroken = false;

    [SerializeField]
    protected bool DestoryOnFailure = false;
    protected int DestroyDelay = 5000;

    public int failureIncrement = 10;
    public int failureSafePoint = 70;
    public int triggerFailureDelay = 2000;

    public event Action OnFailure;

    protected override void Start()
    {
        base.Start();

        OnActivationDown += AdvanceFailure;
        // OnFailure += Shrink;
    }

    public override void ActivateKeyDown(NetworkObjectReference holderRef)
    {
        if (IsBroken) return;
        base.ActivateKeyDown(holderRef);
    }

    public override void ActivateKeyUp(NetworkObjectReference holderRef)
    {
        if (IsBroken) return;
        base.ActivateKeyUp(holderRef);
    }

    protected void AdvanceFailure(IHolder holder)
    {
        // Debug.Log("Failure Prob: " + failureProbability.Value);
        // if (failureSafePoint < failureProbability.Value)
        // {
        //     int failValue = UnityEngine.Random.Range(0, 120);

        //     Debug.Log("Failure Value: " + failValue);
        //     if (failValue < failureProbability.Value)
        //         Fail();
        // }

        // failureProbability.Value += failureIncrement;
        // failureProbability.Value = Math.Clamp(failureProbability.Value, 0, 100);
    }

    protected void Fail()
    {
        TriggerFailureServerRpc();
    }

    protected async Task TriggerFailure()
    {
        // Debug.LogWarning("Failure Trigger");
        await Task.Delay(triggerFailureDelay);
        OnFailure?.Invoke();

        if (DestoryOnFailure) DestroyPrototypeAsync();
    }

    protected virtual async void DestroyPrototypeAsync()
    {
        if (transform.TryGetComponent(out Rigidbody rigidbody)) rigidbody.isKinematic = true;
        transform.localScale = Vector3.zero;
        Holder?.DropObject();

        await Task.Delay(DestroyDelay);
        NetworkObject.Despawn();
    }

    // protected void Shrink()
    // {
    //     transform.localScale = Vector3.zero;
    // }

    #region RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void TriggerFailureServerRpc()
    {
        TriggerFailureClientRpc();
    }

    [ClientRpc]
    protected void TriggerFailureClientRpc()
    {
        IsBroken = true;
        _ = TriggerFailure();
    }

    #endregion
}