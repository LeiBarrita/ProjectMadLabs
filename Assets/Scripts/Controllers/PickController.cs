using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickController
{
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
        Physics.Raycast(cameraRay, 5f);
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10, Color.red);

        // Debug.DrawRay(_hand.position, _hand.forward * 30, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ray!");
        }
    }
}
