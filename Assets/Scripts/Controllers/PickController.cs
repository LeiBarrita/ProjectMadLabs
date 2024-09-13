using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickController : NetworkBehaviour
{
    [SerializeField] private KeyCode PickKey;
    private Item holdingItem;

    private void Update()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(cameraRay.origin, cameraRay.direction * 3, Color.red);

        if (Physics.Raycast(cameraRay, out RaycastHit hitObject, 3f))
        {
            Item pickObject = hitObject.transform.gameObject.GetComponent<Item>();

            if (pickObject != null && Input.GetKeyDown(PickKey))
            {
                if (holdingItem != null)
                    DropItem();
                else
                    PickItem(pickObject);
            }
        }
    }

    private void PickItem(Item item)
    {
        if (transform.parent.TryGetComponent<Player>(out var player))
        {
            item.OnPick(player, transform);
        }
    }

    private void DropItem()
    {
        holdingItem.OnDrop();
        holdingItem = null;
    }
}
