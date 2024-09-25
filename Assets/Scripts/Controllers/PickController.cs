using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PickController : NetworkBehaviour
{
    [SerializeField] private float raycastRange = 3f;
    [SerializeField] private KeyCode PickKey;
    [SerializeField] private KeyCode ActivateKey;

    private IHolder playerHolder;
    private Camera mainCamera;

    private void Awake()
    {
        playerHolder = transform.GetComponent<IHolder>();
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
            if (playerHolder.PickedObject != null)
            {
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
                playerHolder.PickObject(pickObject);
            }
        }
    }


}
