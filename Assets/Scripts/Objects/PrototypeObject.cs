using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrototypeObject : ActivableObject
{
    public float failureProbability = 0;

    public event Action OnFailure;

    protected void CheckFailure()
    {
        if (failureProbability > 70)
            OnFailure?.Invoke();
    }
}
