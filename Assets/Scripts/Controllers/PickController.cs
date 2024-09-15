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
        if (!IsOwner) return;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 3, Color.red);

        if (Input.GetKeyDown(PickKey))
        {
            if (holdingItem != null)
                DropItem();
            else if (Physics.Raycast(cameraRay, out RaycastHit hitObject, 3f))
            {
                if (hitObject.transform.gameObject.TryGetComponent<Item>(out var pickObject))
                {
                    if (holdingItem != null) DropItem();
                    PickItem(pickObject);
                }
            }
        }
    }

    private void PickItem(Item item)
    {
        if (transform.parent.TryGetComponent<Player>(out var player))
        {
            holdingItem = item;
            item.Pick(NetworkObject);
        }
    }

    private void DropItem()
    {
        Debug.LogWarning("Dropping item");
        holdingItem.Drop();
        holdingItem = null;
    }
}
