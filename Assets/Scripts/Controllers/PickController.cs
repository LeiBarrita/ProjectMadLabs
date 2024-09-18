using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PickController : NetworkBehaviour
{
    [SerializeField] private float raycastRange = 3f;
    [SerializeField] private KeyCode PickKey;
    [SerializeField] private KeyCode ActivateKey;

    private PickableObject holdingObject;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsOwner || mainCamera == null) return;

        HandlePickInput();
        HandleActivateInput();
    }

    private void HandlePickInput()
    {
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

    private void HandleActivateInput()
    {
        if (holdingObject is ActivableObject activableObject)
        {
            if (Input.GetKeyDown(ActivateKey))
                activableObject.ActivateKeyDown();
            if (Input.GetKeyUp(ActivateKey))
                activableObject.ActivateKeyUp();
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
        if (holdingObject != null) return;

        holdingObject = pickableObject;
        pickableObject.Pick(NetworkObject);
    }

    private void DropObject()
    {
        holdingObject.Drop();
        holdingObject = null;
    }
}
