using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickController : NetworkBehaviour
{
    [SerializeField] private float raycastRange = 3f;
    [SerializeField] private KeyCode PickKey;
    [SerializeField] private KeyCode StoreKey;
    [SerializeField] private KeyCode ActivateKey;
    [SerializeField] private KeyCode[] InventoryKeys;

    private string selectedInventorySpace = "0";
    private IObjectKeeper playerKeeper;
    private IHolder playerHolder;
    private Camera mainCamera;

    private void Awake()
    {
        playerKeeper = transform.GetComponent<IObjectKeeper>();
        playerHolder = transform.GetComponent<IHolder>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsOwner || mainCamera == null) return;

        // Debug.Log(!!playerHolder.PickedObject);
        HandlePickInput();
        HandleActivateInput();
        HandleStoreInput();
        HandleInventorySpaceInput();
    }

    private void HandlePickInput()
    {
        if (Input.GetKeyDown(PickKey))
        {
            if (playerHolder.PickedObject != null)
            {
                // Debug.Log("Object Dropped");
                playerHolder.DropObject();
            }
            else
            {
                TryPickObject();
            }
        }
    }

    private void HandleActivateInput()
    {
        if (playerHolder.PickedObject is ActivableObject activableObject)
        {
            // Debug.Log("Picked Object is Activable");
            if (Input.GetKeyDown(ActivateKey))
                activableObject.ActivateKeyDown();
            if (Input.GetKeyUp(ActivateKey))
                activableObject.ActivateKeyUp();
        }
    }

    private void HandleInventorySpaceInput()
    {
        for (int i = 0; i < InventoryKeys.Length; i++)
        {
            if (!Input.GetKeyDown(InventoryKeys[i])) continue;
            selectedInventorySpace = i.ToString();
            break;
        }
    }

    private void HandleStoreInput()
    {
        if (Input.GetKeyDown(StoreKey))
        {
            if (playerHolder.PickedObject != null)
            {
                playerKeeper.TryStorePickedObject(selectedInventorySpace);
            }
            else
            {
                playerKeeper.TryExtractObject(selectedInventorySpace);
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
                playerHolder.PickObject(pickObject);
            }
        }
    }


}
