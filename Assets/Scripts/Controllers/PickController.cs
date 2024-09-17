using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PickController : NetworkBehaviour
{
    [SerializeField] private float raycastRange = 3f;
    [SerializeField] private KeyCode PickKey;

    private PickableObject holdingObject;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsOwner || mainCamera == null) return;

        if (Input.GetKeyDown(PickKey))
        {
            if (holdingObject != null)
            {
                DropObject();
            }
            else
            {
                TryPickObject();
            }
        }
    }

    private void TryPickObject()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraRay, out RaycastHit hitObject, raycastRange))
        {
            if (hitObject.transform.TryGetComponent(out PickableObject pickObject))
            {
                PickObject(pickObject);
            }
        }
    }

    private void PickObject(PickableObject pickableObject)
    {
        if (holdingObject == null)
        {
            holdingObject = pickableObject;
            pickableObject.Pick(NetworkObject);
        }
    }

    private void DropObject()
    {
        holdingObject.Drop();
        holdingObject = null;
    }
}
