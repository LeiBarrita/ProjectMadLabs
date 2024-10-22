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
    [SerializeField] private KeyCode FuelActionKey;
    [SerializeField] private KeyCode[] InventoryKeys;

    private int selectedInventorySpace = 0;
    private IObjectKeeper playerKeeper;
    private Player player;
    private Camera mainCamera;

    private void Awake()
    {
        playerKeeper = transform.GetComponent<IObjectKeeper>();
        player = transform.GetComponent<Player>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsOwner || mainCamera == null) return;

        HandlePickInput();
        HandleActivateInput();
        HandleStoreInput();
        HandleInventorySpaceInput();
    }

    private void HandlePickInput()
    {
        if (Input.GetKeyDown(PickKey))
        {
            if (Input.GetKey(FuelActionKey))
            {
                player.DropFuelAction();
                return;
            }

            if (player.Holder.PickedObject != null)
            {
                player.DropAction();
                return;
            }

            TryPickObject();
        }
    }

    private void HandleActivateInput()
    {
        if (player.Holder.PickedObject is ActivableObject activableObject)
        {
            if (Input.GetKeyDown(ActivateKey))
                activableObject.ActivateKeyDown(player.Holder.HolderRef);
            if (Input.GetKeyUp(ActivateKey))
                activableObject.ActivateKeyUp(player.Holder.HolderRef);
        }
    }

    private void HandleInventorySpaceInput()
    {
        for (int i = 0; i < InventoryKeys.Length; i++)
        {
            if (!Input.GetKeyDown(InventoryKeys[i])) continue;
            selectedInventorySpace = i;
            break;
        }
    }

    private void HandleStoreInput()
    {
        if (Input.GetKeyDown(StoreKey))
        {
            if (player.Holder.PickedObject != null)
            {
                // playerKeeper.TryStorePickedObject(selectedInventorySpace);
                player.Keeper.StoreAction(player.Holder, selectedInventorySpace);
            }
            else
            {
                // playerKeeper.TryExtractObject(selectedInventorySpace);
                player.Keeper.ExtractAction(player.Holder, selectedInventorySpace);
            }
        }
    }

    private void TryPickObject()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraRay, out RaycastHit hitObject, raycastRange))
        {
            if (hitObject.transform.TryGetComponent(out Fuel fuel))
            {
                player.PickFuelAction(fuel);
                return;
            }
            if (hitObject.transform.TryGetComponent(out PickableObject pickObject))
            {
                player.PickAction(pickObject);
                return;
            }
        }
    }


}
