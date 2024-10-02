using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
    public event Action<IHolder> OnHold; // Action trigger when something takes this object
    public event Action<IHolder> OnDrop; // Action trigger when the Holder loses this object

    public IHolder Holder;
    // public bool CanShrink;

    private Transform FollowPosition;
    private Rigidbody rb;

    // Temporal
    protected virtual void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.freezeRotation = true;
        rb.isKinematic = false;
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

    public virtual void Store(NetworkObjectReference keeperRef)
    {
        StoreItemServerRpc(keeperRef);
    }

    public virtual void Extract()
    {
        ExtractItemServerRpc();
    }

    protected virtual void RemoveCurrentHolder(IHolder currentHolder)
    {
        Debug.Log("PickableObject -> RemoveCurrentHolder -> Start: { Holder: " + (Holder != null) + ", currentHolder: " + (currentHolder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");

        Holder = null;
        FollowPosition = null;

        rb.isKinematic = false;

        currentHolder.DropObject();

        Debug.Log("PickableObject -> RemoveCurrentHolder -> End: { Holder: " + (Holder != null) + ", currentHolder: " + (currentHolder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");
    }

    protected virtual void SetNewHolder(IHolder newHolder)
    {
        Debug.Log("PickableObject -> SetNewHolder -> Start: { Holder: " + (Holder != null) + ", newHolder: " + (newHolder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");

        Holder = newHolder;
        FollowPosition = newHolder.HoldTransform;
        rb.isKinematic = true;

        newHolder.PickObject(this);

        Debug.Log("PickableObject -> SetNewHolder -> End: { Holder: " + (Holder != null) + ", newHolder: " + (newHolder != null) + ", FollowPosition: " + (FollowPosition != null) + " }");
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
        if (!networkObject.transform.TryGetComponent(out IHolder newHolder)) return;

        Debug.Log(
            "PickableObject -> PickItemClientRpc -> Start: { Holder: "
            + (Holder != null) + ", newHolder: "
            + (newHolder != null) + ", FollowPosition: "
            + (FollowPosition != null) + " }"
        );

        if (Holder != null && Holder != newHolder)
        {
            OnDrop?.Invoke(Holder);
            Holder.DropObject();
        }

        SetNewHolder(newHolder);
        OnHold?.Invoke(newHolder);

        Debug.Log("PickableObject -> PickItemClientRpc -> End: { Holder: "
            + (Holder != null) + ", newHolder: "
            + (newHolder != null) + ", FollowPosition: "
            + (FollowPosition != null) + " }"
        );
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
        if (!networkObject.transform.TryGetComponent(out IHolder currentHolder)) return;

        Debug.Log("PickableObject -> DropItemClientRpc -> Start: { Holder: "
            + (Holder != null) + ", currentHolder: "
            + (currentHolder != null) + ", FollowPosition: "
            + (FollowPosition != null) + " }"
        );

        OnDrop?.Invoke(currentHolder);
        RemoveCurrentHolder(currentHolder);

        Debug.Log("PickableObject -> DropItemClientRpc -> End: { Holder: "
            + (Holder != null) + ", currentHolder: "
            + (currentHolder != null) + ", FollowPosition: "
            + (FollowPosition != null) + " }"
        );
    }

    [ServerRpc(RequireOwnership = false)]
    protected void StoreItemServerRpc(NetworkObjectReference keeperRef)
    {
        StoreItemClientRpc(keeperRef);
    }

    [ClientRpc]
    protected void StoreItemClientRpc(NetworkObjectReference keeperRef)
    {
        if (!keeperRef.TryGet(out NetworkObject networkObject)) return;
        if (!networkObject.transform.TryGetComponent(out IObjectKeeper keeper)) return;

        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<Collider>().enabled = false;

        if (keeper is IHolder holder) FollowPosition = holder.HolderTransform;

    }

    [ServerRpc(RequireOwnership = false)]
    protected void ExtractItemServerRpc()
    {
        ExtractItemClientRpc();
    }

    [ClientRpc]
    protected void ExtractItemClientRpc()
    {
        transform.GetComponent<Renderer>().enabled = true;
        transform.GetComponent<Collider>().enabled = true;
    }

    #endregion
}
