using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickController : MonoBehaviour
{
    [SerializeField] private KeyCode PickKey;

    private void Update()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
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
