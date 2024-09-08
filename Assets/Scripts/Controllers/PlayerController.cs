using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("Movement")]
    // public Transform orientation;
    [SerializeField] private float moveSpeed = 1f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }

    void Update()
    {
        if (!IsOwner) return;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
        rigidbody.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
    }
}
