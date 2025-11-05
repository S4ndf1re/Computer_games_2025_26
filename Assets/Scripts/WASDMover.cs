using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput), typeof(Collider))]
public class WASDMover : MonoBehaviour
{
    [Header("Speed")]
    public float moveSpeed = 10f;
    public float jumpForce = 50f;

    [Header("Ground Check")]
    public LayerMask groundMask;
    public float groundMargin = 0.05f;

    [Header("Coyote Jump")]
    public float coyoteTime = 0.15f;
    private float coyoteTimer = 0f;

    [Header("Optional")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Collider col;

    private Vector2 moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player2D");

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        col = GetComponent<Collider>();
    }

    void OnEnable()
    {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction != null && jumpAction.WasPressedThisFrame())
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        Bounds b = col.bounds;
        float rayLen = b.extents.y + groundMargin;
        
        bool groundHit = Physics.Raycast(
            b.center, Vector3.down, rayLen,
            groundMask, QueryTriggerInteraction.Ignore);

        if (groundHit)
        {
            isGrounded = true;
            coyoteTimer = coyoteTime;
        }
        else
        {
            isGrounded = false;
            coyoteTimer -= Time.fixedDeltaTime;
        }

        Vector3 fwd = Vector3.forward;
        Vector3 right = Vector3.right;
        if (cameraTransform)
        {
            Vector3 cf = cameraTransform.forward; cf.y = 0; cf.Normalize();
            Vector3 cr = cameraTransform.right;   cr.y = 0; cr.Normalize();
            fwd = cf; right = cr;
        }

        Vector3 moveDir = fwd * moveInput.y + right * moveInput.x;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        bool canJump = (isGrounded || coyoteTimer > 0f);

        if (jumpRequested && canJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            jumpRequested = false;
            coyoteTimer = 0f;
        }
        else
        {
            jumpRequested = false;
        }
    }
}
