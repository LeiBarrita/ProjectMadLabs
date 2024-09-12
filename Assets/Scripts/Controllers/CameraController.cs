using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sens;

    private Transform headTrans;
    private Transform playerTrans;
    private float _xRotation;
    private float _yRotation;

    // private PickController pickControl;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        if (headTrans != null) transform.position = headTrans.position;
        if (playerTrans != null) playerTrans.rotation = Quaternion.Euler(0, _yRotation, 0);

        // pickControl?.Controls();
    }

    public void onPlayerSpawns(Component sender, object data)
    {
        if (sender is Player player)
        {
            if (player.IsOwner)
            {
                playerTrans = player.transform;
                headTrans = player.transform.Find("Head");

                player.OnPlayerDestroy += OnParentDestroy;

                // pickControl = new PickController(player.transform.Find("RightHand"), Camera.main);

                Debug.LogWarning("Is Owner!");
            }
        }
    }

    void OnParentDestroy()
    {
        // transform.parent = null;
        headTrans = transform;
        // pickControl = null;
        Debug.LogWarning("Main Camera parent destroyed");
    }
}
