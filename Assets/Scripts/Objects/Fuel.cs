using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fuel : NetworkBehaviour
{
    public IFuelHolder FuelHolder;
    private Transform FollowPosition;
    // private Rigidbody rb;

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
            followPos.rotation * Quaternion.Euler(0, 0, 90)
        );
    }

    public virtual void Pick(NetworkObjectReference holderRef)
    {
        PickFuelServerRpc(holderRef);
    }

    public virtual void Drop(NetworkObjectReference holderRef)
    {
        DropFuelServerRpc(holderRef);
    }

    protected virtual void SetNewFuelHolder(IFuelHolder newFuelHolder)
    {
        FuelHolder?.RemoveFuelReference(this);

        FuelHolder = newFuelHolder;
        FollowPosition = newFuelHolder.FuelHoldSpace;

        Debug.Log("sdnkjsak");
        // rb.isKinematic = true;

        // newFuelHolder.PickFuel(this);
    }

    protected virtual void RemoveCurrentFuelHolder(IFuelHolder currentFuelHolder)
    {
        FuelHolder = null;
        FollowPosition = null;
        // rb.isKinematic = false;
    }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void PickFuelServerRpc(NetworkObjectReference holderRef)
    {
        PickFuelClientRpc(holderRef);
    }

    [ClientRpc]
    protected void PickFuelClientRpc(NetworkObjectReference holderRef)
    {

        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IFuelHolder newFuelHolder)) return;

        // if (FuelHolder != null && FuelHolder != newFuelHolder)
        // {
        //     OnDrop?.Invoke(FuelHolder);
        //     FuelHolder.DropObject();
        // }

        SetNewFuelHolder(newFuelHolder);
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DropFuelServerRpc(NetworkObjectReference holderRef)
    {
        DropFuelClientRpc(holderRef);
    }

    [ClientRpc]
    protected void DropFuelClientRpc(NetworkObjectReference holderRef)
    {
        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IFuelHolder currentHolder)) return;

        RemoveCurrentFuelHolder(currentHolder);
    }

    #endregion
}
