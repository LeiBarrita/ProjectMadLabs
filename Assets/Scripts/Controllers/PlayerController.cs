using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    #region ApplyMovement

    [Header("ApplyMovement")]
    [SerializeField] private float baseSpeed = 2000f;
    [SerializeField] private float airSpeedMultiplier = 0.2f;
    [SerializeField] private float sprintSpeedIncrement = 2f;
    [SerializeField] private float crouchSpeedDecrement = 0.5f;
    [SerializeField] private float groundDrag = 5f;
    private float moveSpeed;

    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private int jumpCooldown = 200;
    private bool canJump = true;

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
        bool isSprinting = Input.GetKey(sprintKey);
        bool isCrouching = Input.GetKey(crouchKey);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (!Grounded) return;

        if (Input.GetKeyDown(jumpKey) && canJump)
        {
            LockTimerJumpAsync(jumpCooldown);
            Grounded = false;
            Jump();

            Task.Delay(jumpCooldown);

            return;
        }

        if (isSprinting) moveSpeed = baseSpeed * sprintSpeedIncrement;
        else if (isCrouching) moveSpeed = baseSpeed * crouchSpeedDecrement;
        else if (moveSpeed != baseSpeed) moveSpeed = baseSpeed;
    }

    private void ApplyMovement()
    {
        verticalInput = MathF.Round(verticalInput);
        horizontalInput = MathF.Round(horizontalInput);

        if (verticalInput < 1) StopDirectionVelocity(transform.forward);
        if (verticalInput > -1) StopDirectionVelocity(-transform.forward);
        if (horizontalInput < 1) StopDirectionVelocity(transform.right);
        if (horizontalInput > -1) StopDirectionVelocity(-transform.right);

        moveDir = transform.forward * verticalInput + transform.right * horizontalInput;
        rb.AddForce(moveSpeed * moveDir.normalized, ForceMode.Force);

        // Debug.Log(verticalInput);
        // Debug.Log(moveDir);
        Debug.Log(Vector3.Dot(rb.velocity, transform.forward));
        // Debug.Log(transform.forward);
        // Debug.Log("Direction: " + moveDir.normalized + "Base Speed: " + baseSpeed + " Move Speed: " + moveSpeed);
    }

    private void StopDirectionVelocity(Vector3 direction)
    {
        float forwardSpeed = Vector3.Dot(rb.velocity, direction);
        float minSpeed = 0.2f;

        if (forwardSpeed > minSpeed && Grounded)
        {
            Vector3 forwardVelocity = direction * forwardSpeed;
            rb.velocity -= forwardVelocity;
            // Debug.Log("Stopped");
        }
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
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private async void LockTimerJumpAsync(int lockTime)
    {
        canJump = false;
        await Task.Delay(lockTime);
        canJump = true;
    }

    #endregion
}
