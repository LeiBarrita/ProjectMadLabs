using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public abstract class PrototypeObject : ActivableObject
{
    private NetworkVariable<int> failureProbability = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private bool IsBroken = false;
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

    public override void ActivateKeyDown()
    {
        if (IsBroken) return;
        base.ActivateKeyDown();
    }

    public override void ActivateKeyUp()
    {
        if (IsBroken) return;
        base.ActivateKeyUp();
    }

    protected void AdvanceFailure(IHolder holder)
    {
        UnityEngine.Debug.Log("Failure Prob: " + failureProbability.Value);
        if (failureSafePoint < failureProbability.Value)
        {
            int failValue = UnityEngine.Random.Range(0, 120);

            UnityEngine.Debug.Log("Failure Value: " + failValue);
            if (failValue < failureProbability.Value)
                Fail();
        }

        failureProbability.Value += failureIncrement;
        failureProbability.Value = Math.Clamp(failureProbability.Value, 0, 100);
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

        await Task.Delay(5000);
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