using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public abstract class PrototypeObject : ActivableObject
{
    private NetworkVariable<int> failureProbability = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public int failureIncrement = 10;
    public int failureSafePoint = 70;

    public event Action OnFailure;

    protected override void Start()
    {
        base.Start();

        OnActivationDown += AdvanceFailure;
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

    #region RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void TriggerFailureServerRpc()
    {
        TriggerFailureClientRpc();
    }

    [ClientRpc]
    protected void TriggerFailureClientRpc()
    {
        OnFailure?.Invoke();
    }

    #endregion
}