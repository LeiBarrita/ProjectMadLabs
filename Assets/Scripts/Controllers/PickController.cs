using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickController
{
    [SerializeField]
    private LayerMask layer;
    private readonly Camera _camera;
    private readonly Transform _hand;

    public PickController(Transform hand, Camera camera)
    {
        _camera = camera;
        _hand = hand;
    }

    public void Controls()
    {
        Ray cameraRay = _camera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 3, Color.red);

        if (Physics.Raycast(cameraRay, out RaycastHit hitObject, 3f))
        {
            IPickable pickObject = hitObject.transform.gameObject.GetComponent<IPickable>();

            if (pickObject != null && Input.GetKeyDown(KeyCode.E))
            {
                pickObject.Activate();
            }
        }
    }
}
