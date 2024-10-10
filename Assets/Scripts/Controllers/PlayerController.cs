using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    #region ApplyMovement

    [Header("ApplyMovement")]
    [SerializeField] private float baseSpeed = 80f;
    [SerializeField] private float airSpeedMultiplier = 0.2f;
    [SerializeField] private float sprintSpeedIncrement = 2f;
    [SerializeField] private float crouchSpeedDecrement = 0.5f;
    [SerializeField] private float groundDrag = 10f;
    private float moveSpeed = 0;

    [SerializeField] private float jumpForce;

    #endregion

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    #region Ground Check

    [Header("Ground Check")]
    public float playerHeight;
    public Transform groundCheck;
    public LayerMask ground;
    private bool _grounded = true;
    private bool Grounded
    {
        get { return _grounded; }
        set
        {
            ChangeMovementSpeed(_grounded, value);
            _grounded = value;
        }
    }

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
    }

    void Update()
    {
        if (!IsOwner) return;

        Debug.DrawRay(groundCheck.position, Vector3.down * playerHeight, Color.green);
        Grounded = Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            playerHeight,
            ground
        );

        MovementInput();
        // SpeedControl();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #endregion
    #region Private Funcs

    private void MovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (!Grounded) return;

        if (Input.GetKey(jumpKey))
        {
            Grounded = false;
            Jump();
            return;
        }

        if (Input.GetKey(sprintKey)) moveSpeed = baseSpeed * sprintSpeedIncrement;
        else if (Input.GetKey(crouchKey)) moveSpeed = baseSpeed * crouchSpeedDecrement;
        else if (moveSpeed != baseSpeed) moveSpeed = baseSpeed;
    }

    private void ApplyMovement()
    {
        // rb.velocity
        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
        rb.AddForce(moveSpeed * moveDir.normalized, ForceMode.Force);

        // Debug.Log("Direction: " + moveDir.normalized + "Base Speed: " + baseSpeed + " Move Speed: " + moveSpeed);
    }

    private void ChangeMovementSpeed(bool currentValue, bool newValue)
    {
        if (currentValue == newValue) return;

        if (newValue)
        {
            rb.drag = groundDrag;
            moveSpeed = baseSpeed;
        }
        else
        {
            rb.drag = 0;
            moveSpeed = baseSpeed * airSpeedMultiplier;
        }
        // Debug.Log(currentValue + " " + newValue);
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
        // rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    #endregion
}
