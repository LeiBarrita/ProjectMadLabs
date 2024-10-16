using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : PickableObject
{
    void Start()
    {
        IsStorable = false;
    }

    protected override void SetNewHolder(IHolder newHolder)
    {
        // base.SetNewHolder(newHolder);
        Holder = newHolder;
        FollowPosition = newHolder.HoldTransform;
        // rb.isKinematic = true;

        // newHolder.PickObject(this);
    }
}
