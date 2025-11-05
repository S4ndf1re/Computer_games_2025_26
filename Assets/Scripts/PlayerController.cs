using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
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
    public LayerMask wallLayer;
    public float wallSlideMaxTime = 2f; // <--- NEU

    // intern
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canWallJump;
    private float wallSlideStartTime; // <--- NEU

    private Vector3 lastWallNormal;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float xRotation = 0f;

    private bool isJumping;
    private bool isHoldingJump;
    private float jumpTimeCounter;
    private float lastGroundedTime;

    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float lastDashTime = -999f;
    private Vector3 dashDirection;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleWallSlide();
    }

    void HandleMovement()
    {
        bool grounded = controller.isGrounded;

        // Bodencheck
        if (grounded)
        {
            lastGroundedTime = Time.time;
            if (!isJumping && velocity.y < 0)
                velocity.y = -2f;
        }

        // Bewegung (lokal)
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // DASH
        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            if (Time.time >= dashEndTime)
                isDashing = false;
            return;
        }

        // Gravitation
        float currentGravity = gravity;
        if (isHoldingJump && isJumping && jumpTimeCounter < maxJumpTime)
        {
            currentGravity *= holdJumpGravityMultiplier;
            jumpTimeCounter += Time.deltaTime;
        }

        // Wenn wallslide aktiv ist, fall langsamer
        if (isWallSliding)
        {
            velocity.y = Mathf.Max(velocity.y, -wallSlideSpeed);
            canWallJump = true;
        }
        else
        {
            velocity.y += currentGravity * Time.deltaTime;
        }

        // Bewegung
        Vector3 totalMove = move * moveSpeed + new Vector3(0, velocity.y, 0);
        controller.Move(totalMove * Time.deltaTime);
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
        if (!controller.isGrounded && wallHit && velocity.y < 0)
        {
            if (!isWallSliding)
                wallSlideStartTime = Time.time;

            isWallSliding = true;

            // Timer prüfen – nach Ablauf deaktivieren
            if (Time.time - wallSlideStartTime > wallSlideMaxTime)
                isWallSliding = false;
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
            if (isWallSliding)
            {
                isWallSliding = false;
                isJumping = true;
                isHoldingJump = false;

                // Richtung = Kombination aus Wandnormalen (weg von der Wand) + nach oben
                Vector3 jumpDir = (lastWallNormal + Vector3.up).normalized;

                velocity = jumpDir * wallJumpForce;

                return;
            }
            
            // NORMAL JUMP
            if (controller.isGrounded || Time.time - lastGroundedTime <= coyoteTime)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
