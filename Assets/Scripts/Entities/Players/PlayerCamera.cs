using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    // public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        // orientation = Transform.pare
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // if (orientation != null)
        //     orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void onPlayerSpawns(Component sender, object data)
    {
        if (sender is BasePlayer player)
        {
            if (player.IsOwner)
            {
                // orientation = player.transform;
                transform.parent = player.transform;
                transform.position = Vector3.zero;
                Debug.LogWarning("Is Owner!");
            }
        }
    }
}
