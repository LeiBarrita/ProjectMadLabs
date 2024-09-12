using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickController : NetworkBehaviour
{
    [SerializeField] private KeyCode PickKey;
    // private IPickable pickObject;
    private bool isHolding;

    private void Update()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 3, Color.red);

        if (Physics.Raycast(cameraRay, out RaycastHit hitObject, 3f))
        {
            IPickable pickObject = hitObject.transform.gameObject.GetComponent<IPickable>();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isHolding)
                {
                    isHolding = false;
                }

                Player player = transform.parent.GetComponent<Player>();
                if (player != null)
                {
                    isHolding = true;
                    pickObject.Pick(player, transform);
                }
            }
        }
    }
}
