using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    #region Movement

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float crouchMultiplier = 0.5f;
    private float moveSpeed = 0;
    [SerializeField] private float groundDrag = 1f;

    [SerializeField] private float jumpForce;
    // [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplayer;
    private bool readyToJump = true;

    #endregion

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    #region Ground Check

    [Header("Ground Check")]
    // public float playerHeight;
    private Transform groundCheck;
    public LayerMask ground;
    private bool grounded = true;

    #endregion

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDir;
    Rigidbody rb;

    #region Behaviour Funcs

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = baseSpeed;

        // groundCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        if (!IsOwner) return;

        // grounded = Physics.Raycast(
        //     transform.position,
        //     Vector3.down,
        //     playerHeight * 0.5f + 0.2f,
        //     ground
        // );

        InputRead();

        SpeedControl();

        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    #endregion
    #region Private Funcs

    private void InputRead()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Debug.Log("ready: " + readyToJump);
        // Debug.Log("grounded: " + grounded);

        if (Input.GetKey(jumpKey) && grounded)
        {
            // Debug.Log("Jump Press!");
            // readyToJump = false;
            grounded = false;
            Jump();
            // readyToJump = true;
        }

        if (Input.GetKeyDown(sprintKey)) moveSpeed += baseSpeed * sprintMultiplier;
        if (Input.GetKeyUp(sprintKey)) moveSpeed -= baseSpeed * sprintMultiplier;

        if (Input.GetKeyDown(crouchKey)) moveSpeed -= baseSpeed * crouchMultiplier;
        if (Input.GetKeyUp(crouchKey)) moveSpeed += baseSpeed * crouchMultiplier;
    }

    private void Movement()
    {
        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;

        rb.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Debug.Log("Jumping!");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // private void ResetJump()
    // {
    //     readyToJump = true;
    // }

    #endregion
}
