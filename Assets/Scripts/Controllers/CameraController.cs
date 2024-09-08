using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sens;
    // [SerializeField] private float sensY;

    // public Transform orientation;

    private Transform headTrans;
    private Transform playerTrans;
    private float _xRotation;
    private float _yRotation;

    void Start()
    {
        // orientation = Transform.pare
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        // if (orientation != null) orientation.rotation = Quaternion.Euler(0, _yRotation, 0);

        if (headTrans != null) transform.position = headTrans.position;
        if (playerTrans != null) playerTrans.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    public void onPlayerSpawns(Component sender, object data)
    {
        if (sender is Player player)
        {
            if (player.IsOwner)
            {
                // orientation = player.transform;

                // transform.parent = player.transform.Find("Head");
                // transform.localPosition = Vector3.zero;

                playerTrans = player.transform;
                headTrans = player.transform.Find("Head");

                player.OnPlayerDestroy += OnParentDestroy;

                Debug.LogWarning("Is Owner!");
            }
        }
    }

    void OnParentDestroy()
    {
        // transform.parent = null;
        headTrans = transform;
        Debug.LogWarning("Main Camera parent destroyed");
    }
}
