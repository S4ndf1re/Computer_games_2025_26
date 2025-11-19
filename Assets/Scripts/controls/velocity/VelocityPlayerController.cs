using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Velocity))]
public class VelocityPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float airSpeed = 200f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float maxJumpTime = 0.35f;
    public float holdJumpGravityMultiplier = 0.3f;

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
    public float wallSlideMaxTime = 2f; // <--- NEU

    // intern
    private bool isTouchingWall;
    private bool isWallSliding;
    public bool canWallJump;
    private float wallSlideStartTime; // <--- NEU

    private Vector3 lastWallNormal;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;

    private bool isJumping;
    private bool isHoldingJump;
    private float jumpTimeCounter;
    private float lastGroundedTime;

    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float lastDashTime = -999f;
    private Vector3 dashDirection;


    private Velocity velocity;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        velocity = GetComponent<Velocity>();
    }

    void Update()
    {
        HandleMovement();
        HandleWallSlide();
    }

    void HandleMovement()
    {

        // Bewegung (lokal)
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (velocity.IsGrounded())
        {
            isJumping = false;
        }

        // DASH
        if (isDashing)
        {
            velocity.DisableGravity();
            velocity.SetInstant(dashDirection * dashSpeed);
            if (Time.time >= dashEndTime)
            {
                velocity.ResetVelocity();
                isDashing = false;
            }
            return;
        }


        // Wenn wallslide aktiv ist, fall langsamer
        if (isWallSliding)
        {
            velocity.DisableGravity();
            velocity.ResetVelocity();
            velocity.SetY(-wallSlideSpeed);
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
            velocity.EnableGravity();
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

    void HandleWallSlide()
    {
        // Raycast in Bewegungsrichtung, um Wand zu finden
        RaycastHit hit;
        bool wallHit = Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance, wallLayer)
                    || Physics.Raycast(transform.position, -transform.forward, out hit, wallCheckDistance, wallLayer)
                    || Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance, wallLayer)
                    || Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance, wallLayer);

        isTouchingWall = wallHit;

        if (wallHit)
            lastWallNormal = hit.normal; // Normale speichern

        // WallSlide aktivieren, wenn in der Luft, Wand berührt und nach unten fällt
        if (!velocity.IsGrounded() && wallHit && velocity.IsFalling())
        {
            if (!isWallSliding)
                wallSlideStartTime = Time.time;

            isWallSliding = true;

            // // Timer prüfen – nach Ablauf deaktivieren
            // if (Time.time - wallSlideStartTime > wallSlideMaxTime)
            //     isWallSliding = false;
        }
        else
        {
            isWallSliding = false;
        }
    }

    // ---------------------------
    // Input Callbacks
    // ---------------------------
    public void OnMove(InputAction.CallbackContext context)
        => moveInput = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        // Sprung starten (Boden, Coyote oder Wand)
        if (context.started)
        {
            // WALL JUMP
            if (canWallJump)
            {
                isWallSliding = false;
                isJumping = true;
                isHoldingJump = false;

                // Richtung = Kombination aus Wandnormalen (weg von der Wand) + nach oben
                Vector3 jumpDir = (lastWallNormal + Vector3.up).normalized;

                velocity.ResetVelocity();
                velocity.AddInstant(jumpDir * wallJumpForce);
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
        }

        // Loslassen beendet verlängerten Sprung
        if (context.canceled)
            isHoldingJump = false;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || isDashing)
            return;

        if (Time.time - lastDashTime < dashCooldown)
            return;

        lastDashTime = Time.time;
        isDashing = true;
        dashEndTime = Time.time + dashDuration;

        // Richtung = aktuelle Bewegung oder Vorwärtsrichtung
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        dashDirection = move.magnitude > 0.1f ? move.normalized : transform.forward;
    }
}
