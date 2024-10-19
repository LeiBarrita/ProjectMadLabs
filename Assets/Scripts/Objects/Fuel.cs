using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fuel : NetworkBehaviour
{
    public IFuelHolder FuelHolder;
    protected Transform FollowPosition;
    // private Rigidbody rb;

    void Start()
    {

    }

    // Temporal
    protected virtual void Awake()
    {
        // rb = transform.GetComponent<Rigidbody>();
        // rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        // rb.freezeRotation = true;
        // rb.isKinematic = false;
    }

    protected virtual void LateUpdate()
    {
        if (FollowPosition == null) return;
        FollowHoldPosition(FollowPosition);
    }

    protected virtual void FollowHoldPosition(Transform followPos)
    {
        transform.SetPositionAndRotation(
            followPos.position,
            followPos.rotation
        );
    }

    public virtual void Pick(NetworkObjectReference holderRef)
    {
        PickItemServerRpc(holderRef);
    }

    public virtual void Drop(NetworkObjectReference holderRef)
    {
        DropItemServerRpc(holderRef);
    }

    protected virtual void RemoveCurrentHolder(IFuelHolder currentFuelHolder)
    {
        FuelHolder = null;
        FollowPosition = null;
        // rb.isKinematic = false;

        currentFuelHolder.DropFuel();
    }

    protected virtual void SetNewHolder(IFuelHolder newFuelHolder)
    {
        FuelHolder = newFuelHolder;
        FollowPosition = newFuelHolder.FuelHoldTransform;
        // rb.isKinematic = true;

        newFuelHolder.PickFuel(this);
    }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void PickItemServerRpc(NetworkObjectReference holderRef)
    {
        PickItemClientRpc(holderRef);
    }

    [ClientRpc]
    protected void PickItemClientRpc(NetworkObjectReference holderRef)
    {

        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IFuelHolder newFuelHolder)) return;

        // if (FuelHolder != null && FuelHolder != newFuelHolder)
        // {
        //     OnDrop?.Invoke(FuelHolder);
        //     FuelHolder.DropObject();
        // }

        SetNewHolder(newFuelHolder);
        // OnHold?.Invoke(newFuelHolder);
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DropItemServerRpc(NetworkObjectReference holderRef)
    {
        DropItemClientRpc(holderRef);
    }

    [ClientRpc]
    protected void DropItemClientRpc(NetworkObjectReference holderRef)
    {
        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IFuelHolder currentHolder)) return;

        // OnDrop?.Invoke(currentHolder);
        RemoveCurrentHolder(currentHolder);
    }

    #endregion
}
