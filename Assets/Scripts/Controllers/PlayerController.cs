using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    #region ApplyMovement

    [Header("ApplyMovement")]
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float airSpeedReduction = 0.2f;
    [SerializeField] private float sprintSpeedIncrement = 2f;
    [SerializeField] private float crouchSpeedDecrement = 0.5f;
    [SerializeField] private float groundDrag = 1f;
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

        // Debug.Log("Base Speed: " + baseSpeed + " Move Speed: " + moveSpeed);
        // moveSpeed = baseSpeed;
        // Debug.Log("Base Speed: " + baseSpeed + " Move Speed: " + moveSpeed);

        // groundCheck = transform.Find("GroundCheck");
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
        // Debug.Log(_grounded);

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
        // if (_grounded) GroundInitialSpeed();
        // else AirInitialSpeed();

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Debug.Log("_grounded: " + _grounded);

        if (!Grounded) return;

        if (Input.GetKey(jumpKey))
        {
            Grounded = false;
            Jump();
            return;
        }

        // if (Input.GetKeyDown(sprintKey)) moveSpeed += baseSpeed * sprintSpeedIncrement;
        // if (Input.GetKeyUp(sprintKey)) moveSpeed -= baseSpeed * sprintSpeedIncrement;
        if (Input.GetKey(sprintKey)) moveSpeed = baseSpeed * sprintSpeedIncrement;

        // if (Input.GetKeyDown(crouchKey)) moveSpeed -= baseSpeed * crouchSpeedDecrement;
        // if (Input.GetKeyUp(crouchKey)) moveSpeed += baseSpeed * crouchSpeedDecrement;
        else if (Input.GetKey(crouchKey)) moveSpeed = baseSpeed * crouchSpeedDecrement;
        else if (moveSpeed != baseSpeed) moveSpeed = baseSpeed;
    }

    private void ApplyMovement()
    {
        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
        Debug.Log("Direction: " + moveDir.normalized + "Base Speed: " + baseSpeed + " Move Speed: " + moveSpeed);

        rb.AddForce(moveSpeed * moveDir.normalized, ForceMode.Force);
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
            moveSpeed = baseSpeed * airSpeedReduction;
        }
        Debug.Log(currentValue + " " + newValue);
    }

    // private void AirInitialSpeed()
    // {
    //     rb.drag = 0;
    //     moveSpeed = baseSpeed * airSpeedReduction;
    // }

    // private void SpeedControl()
    // {
    //     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //     if (flatVel.magnitude > moveSpeed)
    //     {
    //         Vector3 limitedVel = flatVel.normalized * moveSpeed;
    //         rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    //     }
    // }

    private void Jump()
    {
        // rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    #endregion
}
