using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Fuel : PickableObject
{
    public IFuelHolder FuelHolder;
    // private Transform FollowPosition;
    // private Rigidbody rb;

    // Temporal
    // protected virtual void Awake()
    // {
    // rb = transform.GetComponent<Rigidbody>();
    // rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    // rb.freezeRotation = true;
    // rb.isKinematic = false;
    // }

    // protected virtual void LateUpdate()
    // {
    //     if (FollowPosition == null) return;
    //     FollowHoldPosition(FollowPosition);
    // }

    // protected virtual void FollowHoldPosition(Transform followPos)
    protected override void FollowHoldPosition(Transform followPos)
    {
        transform.SetPositionAndRotation(
            followPos.position,
            followPos.rotation * Quaternion.Euler(0, 0, 90)
        );
    }

    public virtual void Pack(NetworkObjectReference holderRef)
    {
        PackFuelServerRpc(holderRef);
    }

    public virtual void Unpack(NetworkObjectReference holderRef)
    {
        UnpackFuelServerRpc(holderRef);
    }

    protected virtual void SetNewFuelHolder(IFuelHolder newFuelHolder)
    {
        FuelHolder = newFuelHolder;
        FollowPosition = newFuelHolder.FuelHoldSpace;
        // rb.isKinematic = true;

        newFuelHolder.PackFuel(this);
    }

    protected virtual void RemoveCurrentFuelHolder(IFuelHolder currentFuelHolder)
    {
        FuelHolder = null;
        FollowPosition = null;
        // rb.isKinematic = false;
        currentFuelHolder.UnpackFuel();
    }

    #region  RPCs

    [ServerRpc(RequireOwnership = false)]
    protected void PackFuelServerRpc(NetworkObjectReference holderRef)
    {
        PackFuelClientRpc(holderRef);
    }

    [ClientRpc]
    protected void PackFuelClientRpc(NetworkObjectReference holderRef)
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
    protected void UnpackFuelServerRpc(NetworkObjectReference holderRef)
    {
        UnpackFuelClientRpc(holderRef);
    }

    [ClientRpc]
    protected void UnpackFuelClientRpc(NetworkObjectReference holderRef)
    {
        if (!holderRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IFuelHolder currentHolder)) return;

        RemoveCurrentFuelHolder(currentHolder);
    }

    #endregion
}
