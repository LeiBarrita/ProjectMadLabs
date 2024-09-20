using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class PrototypeObject : ActivableObject
{
    private NetworkVariable<int> randomNum = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public int failureProbability = 0;
    public int failureIncrement = 10;

    public event Action OnFailure;

    protected override void Start()
    {
        base.Start();

        OnActivationDown += CheckFailure;
    }

    protected void CheckFailure(IHolder holder)
    {
        randomNum.Value = UnityEngine.Random.Range(0, 100);

        if (randomNum.Value < failureProbability)
            OnFailure?.Invoke();
    }
}
