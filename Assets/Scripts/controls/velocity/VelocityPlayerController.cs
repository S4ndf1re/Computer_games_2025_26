using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Velocity))]
public class VelocityPlayerController : MonoBehaviour
{
    /// ---------------------------
    /// Serializable Fields
    /// ---------------------------
    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;

    [Header("Player Model")]
    public Transform playerModel;

    [Header("Movement Settings")]
    public bool canMove = true;
    public float moveSpeed = 5f;
    public float airSpeed = 200f;
    public float jumpHeight = 2f;
    public float maxJumpTime = 0.35f;
    public float holdJumpGravityMultiplier = 0.3f;
    public bool disableZMovement = false;
    public bool disableJump = false;

    [Header("Rotation Settings")]
    public float rotationSmoothTime = 0.1f;
    private float rotationVelocity;
    private float targetRotation;

    [Header("Coyote Time Settings")]
    public float coyoteTime = 0.15f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    [Header("Wall Climb Settings")]
    public float wallCheckDistance = 0.6f;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 7f;
    public Vector2 wallJumpAngle = new Vector2(1f, 1.5f);
    public float inputLockAfterJump = 0.5f;
    public LayerMask wallLayer;
    public float wallSlideMaxTime = 2f;

    [Header("KnockBack")]
    public bool shouldBeKnockedBack = false;
    public float inputLockAfterHit = 0.25f;
    public float knockBackForce = 7f;
    public float knockUpForce = 1f;
    public Hurtbox hurtbox;

    [Header("Ink Settings")]
    public float maxSpeedOnInk = 8f;
    public float overallMaxSpeedBuffer = 8f;
    public float slowedForSeconds = 1f;
    public LayerMask inkLayerMask;
    private Coroutine currentlyInkedRunning;

    [Header("Double Jump Settings")]
    public bool canDoubleJump = true;

    /// ---------------------------
    /// Internal Variables
    /// ---------------------------
    // Movement
    private Velocity velocity;
    private Vector2 moveInput;

    // Rotation
    private Vector2 lookInput;
    private float xRotation = 0f;

    // Dash
    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float lastDashTime = -999f;
    private Vector3 dashDirection;

    // Wall Climb
    private bool isTouchingWall;
    public bool isWallSliding;
    private bool canWallJump;
    private float wallSlideStartTime;
    private Vector3 lastWallNormal;
    private bool isWallJumping = false;
    private float wallJumpEndTime;
    private bool isPressingIntoWall;


    // Jump
    private bool isJumping;
    private bool isHoldingJump;
    private float jumpTimeCounter;
    private float lastGroundedTime;

    // Double Jump
    private float doubleJumpDelayTimer = 0f;
    private bool hasDoubleJumped = false;

    /// ---------------------------
    /// Object Functions
    /// ---------------------------
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        velocity = GetComponent<Velocity>();
        hurtbox = GetComponentInChildren<Hurtbox>();
        if (hurtbox)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
    }

    void Update()
    {
        if (canMove)
        {
            HandleRotation();
            HandleMovement();
            HandleDash();
            HandleWallSlide();
        }
    }

    /// ---------------------------
    /// Internal Functions
    /// ---------------------------

    /// <summary>
    /// Handles Movement and Jumping
    /// </summary>
    void HandleMovement()
    {
        if (isWallJumping && Time.time >= wallJumpEndTime)
        {
            isWallJumping = false;
        }
        // Camera-relative movement
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        // Bewegung (lokal)
        if (disableZMovement)
        {
            move = camRight * moveInput.x;
        }

        if (velocity.IsGrounded())
        {
            lastGroundedTime = Time.time;
            isJumping = false;
            hasDoubleJumped = false;
        }

        // Wenn wallslide aktiv ist, fall langsamer
        if (isWallSliding && !isWallJumping)
        {
            /*
            velocity.DisableGravity();
            velocity.ResetVelocity();
            velocity.SetY(-wallSlideSpeed);
            canWallJump = true;
            */
            velocity.EnableGravity();
            velocity.SetY(-wallSlideSpeed);
            canWallJump = true;
        }
        else
        {
            /*
            canWallJump = false;
            velocity.EnableGravity();
            */
            canWallJump = false;
        }


        // Add final Movement
        if (velocity.IsGrounded())
        {
            velocity.SetInstant(move * moveSpeed);
        }
        else
        {
            // Use delta time here, since this will not reset
            velocity.AddInstant(move * airSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Handles Checking if the player is currently wallsliding
    /// </summary>
    void HandleWallSlide()
    {
        RaycastHit hit;
        isTouchingWall = false;

        Vector3[] dirs =
        {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        foreach (var dir in dirs)
        {
            if (Physics.Raycast(transform.position, dir, out hit, wallCheckDistance, wallLayer))
            {
                isTouchingWall = true;
                lastWallNormal = hit.normal;
                break;
            }
        }

        // Prüfen, ob Input in Richtung Wand geht
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputWorldDir =
            camForward * moveInput.y +
            camRight * moveInput.x;

        // true, wenn Input in Richtung der Wand zeigt
        isPressingIntoWall =
            inputWorldDir.sqrMagnitude > 0.01f &&
            Vector3.Dot(inputWorldDir.normalized, -lastWallNormal) > 0.5f;

        /*
        RaycastHit hit;
        bool wallHit = Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, wallLayer)
                    || Physics.Raycast(transform.position, -transform.forward, out hit, wallCheckDistance, wallLayer)
                    || Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance, wallLayer)
                    || Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance, wallLayer);

        isTouchingWall = wallHit;

        if (wallHit)
            lastWallNormal = hit.normal; // Normale speichern
        */
        // Raycast in Bewegungsrichtung, um Wand zu finden
        
        // WallSlide aktivieren, wenn in der Luft, Wand berührt und nach unten fällt
        if (!velocity.IsGrounded()
            && isTouchingWall
            && velocity.IsFalling()
            && !isWallJumping
            && isPressingIntoWall)
        {
            if (!isWallSliding)
                wallSlideStartTime = Time.time;

            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    /// <summary>
    /// Handles Rotation of Player Model based on move direction
    /// </summary>
    void HandleRotation()
    {
        if (moveInput.sqrMagnitude < 0.01f)
            return;

        if (cameraTransform == null || playerModel == null)
            return;

        // Camera Relative
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;

        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;

        float angle = Mathf.SmoothDampAngle(
            playerModel.eulerAngles.y,
            targetAngle,
            ref rotationVelocity,
            rotationSmoothTime
        );

        playerModel.rotation = Quaternion.Euler(0, angle, 0);
    }

    /// <summary>
    /// Handles Dashing Ability
    /// </summary>
    void HandleDash()
    {
        if (!isDashing) return;

        // ---- DASH START ----
        lastDashTime = Time.time;
        isDashing = true;
        dashEndTime = Time.time + dashDuration;

        // Kamera-basierte Richtung
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Nur horizontaler Input
        Vector3 camMove = camForward * moveInput.y + camRight * moveInput.x;

        // fallback
        if (camMove.sqrMagnitude < 0.01f)
            camMove = camForward;

        // Planar erzwingen
        camMove.y = 0;

        dashDirection = camMove.normalized;

        velocity.SetInstant(dashDirection * dashSpeed);

        isDashing = false;
    }

    /// ---------------------------
    /// Input Callbacks
    /// ---------------------------
    public void OnMove(InputAction.CallbackContext context)
        => moveInput = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        // Sprung starten (Boden, Coyote oder Wand)
        if (context.started && !disableJump && canMove)
        {
            // WALL JUMP
            if (canWallJump)
            {
                isWallSliding = false;
                isWallJumping = true;
                wallJumpEndTime = Time.time + inputLockAfterJump;

                velocity.ResetVelocity();

                Vector3 jumpDir =
                    lastWallNormal.normalized * wallJumpForce +
                    Vector3.up * wallJumpForce * 0.75f;

                velocity.AddInstant(jumpDir);
                velocity.LockInputForSeconds(inputLockAfterJump);
                return;
            }

            // NORMAL JUMP
            if (velocity.IsGrounded() || Time.time - lastGroundedTime <= coyoteTime)
            {
                velocity.Jump(jumpHeight);
                isJumping = true;
                isHoldingJump = true;
                jumpTimeCounter = 0f;
            }

            // Double Jump
            if (!velocity.IsGrounded() && !hasDoubleJumped && !isWallSliding && canDoubleJump)
            {
                hasDoubleJumped = true;
                velocity.Jump(jumpHeight);
                isJumping = true;
                isHoldingJump = true;
                jumpTimeCounter = 0f;
            }
        }

        // Loslassen beendet verlängerten Sprung
        if (context.canceled)
            isHoldingJump = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started || !canMove)
            return;

        // wenn grounded kein dash
        if (velocity.IsGrounded())
            return;

        // wenn man nichts drückt kein dash
        if (moveInput.sqrMagnitude < 0.01f)
            return;

        // wenn man dashed nicht nochmal dashen
        if (isDashing)
            return;

        if (Time.time - lastDashTime < dashCooldown)
            return;

        isDashing = true;
    }

    public void OnHit(Hitbox hitbox)
    {
        if (shouldBeKnockedBack)
        {
            velocity.ResetVelocity();
            Vector3 playerWithoutYZ = new Vector3(transform.position.x, 0, 0);
            Vector3 hitboxWithoutYZ = new Vector3(hitbox.transform.position.x, 0, 0);
            velocity.AddInstant((playerWithoutYZ - hitboxWithoutYZ).normalized * knockBackForce);
            velocity.Jump(knockUpForce);
            velocity.LockInputForSeconds(inputLockAfterHit);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & inkLayerMask) != 0)
        {
            if (currentlyInkedRunning != null)
            {
                StopCoroutine(currentlyInkedRunning);
            }
            currentlyInkedRunning = StartCoroutine(SlowForSeconds());
        }
    }


    IEnumerator SlowForSeconds()
    {
        var oldMaxVelocity = velocity.maxXYGroundSpeed;
        velocity.maxXYGroundSpeed = maxSpeedOnInk;
        if (oldMaxVelocity > maxSpeedOnInk)
        {
            overallMaxSpeedBuffer = oldMaxVelocity;
        }
        yield return new WaitForSeconds(slowedForSeconds);
        velocity.maxXYGroundSpeed = overallMaxSpeedBuffer;
    }
}
