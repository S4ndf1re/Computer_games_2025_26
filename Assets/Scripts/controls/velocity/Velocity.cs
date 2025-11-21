using UnityEngine;

public class Velocity : MonoBehaviour
{

    public Vector3 velocity;
    public float gravity = -9.81f;

    public float maxXYGroundSpeed = 5f;
    public float maxXYAirSpeed = 5f;

    public float maxFallingSpeed = float.PositiveInfinity;

    public LayerMask groundFilter;
    public LayerMask platformFilter;

    [Header("Controller")]
    public CharacterController characterController;


    [Header("Debug")]
    public bool isGrounded;
    public bool gravityDisabled = false;
    private bool inputLocked = false;
    private float lockedTimer = 0.0f;
    private bool previouslyGrounded = true;

    public Transform t;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        previouslyGrounded = IsGrounded();
    }

    void FixedUpdate()
    {
        if (inputLocked)
        {
            lockedTimer -= Time.deltaTime;
            if (lockedTimer <= 0f)
            {
                inputLocked = false;
            }
        }

        // Apply gravity
        if (!gravityDisabled)
        {
            AddY(gravity * Time.deltaTime);
        }


        if (characterController != null)
        {
            characterController.Move(velocity * Time.deltaTime);
        }

        if (IsGrounded() && !IsOnPlattform())
        {
            ResetVelocity();
            // Reset to zero here, since we are using custom check and not collider check
            velocity.y = 0f;
        } else if (IsOnPlattform()) {
            ResetVelocity();
            // Reset to zero here, since we are using custom check and not collider check
            velocity.y = gravity;
        }

        if (!IsGrounded() && previouslyGrounded)
        {
            var oldVelocity = new Vector2(this.velocity.x, this.velocity.z);
            ResetVelocity();
            this.velocity.x = oldVelocity.x;
            this.velocity.z = oldVelocity.y;
        }

        previouslyGrounded = IsGrounded();

        // Debug
        isGrounded = IsGrounded();
    }

    public void AddInstant(Vector3 toAdd)
    {
        if (inputLocked)
        {
            return;
        }
        this.velocity += toAdd;
        ClampVelocity();
    }

    /// <summary>
    /// Setting the velocity in u/s, except in the y direction. The y direction is added to apply gravity
    /// </summary>
    public void SetInstant(Vector3 toAdd)
    {
        if (inputLocked)
        {
            return;
        }
        this.velocity.x = toAdd.x;
        this.velocity.y += toAdd.y;
        this.velocity.z = toAdd.z;

        ClampVelocity();
    }

    public void SetInstant(Vector3 toAdd, bool withoutY)
    {
        if (inputLocked)
        {
            return;
        }
        this.velocity.x = toAdd.x;
        if (!withoutY)
        {
            this.velocity.y += toAdd.y;
        }
        this.velocity.z = toAdd.z;

        ClampVelocity();
    }

    void ClampVelocity()
    {
        if (IsGrounded())
        {
            Vector2 xySpeed = new Vector2(this.velocity.x, this.velocity.z);
            xySpeed = Vector2.ClampMagnitude(xySpeed, maxXYGroundSpeed);

            this.velocity.x = xySpeed.x;
            this.velocity.z = xySpeed.y;
        }
        else
        {
            Vector2 xySpeed = new Vector2(this.velocity.x, this.velocity.z);
            xySpeed = Vector2.ClampMagnitude(xySpeed, maxXYAirSpeed);

            this.velocity.x = xySpeed.x;
            this.velocity.z = xySpeed.y;
        }
        this.velocity.y = Mathf.Clamp(this.velocity.y, -maxFallingSpeed, maxFallingSpeed);
    }

    public void SetY(float y)
    {
        this.velocity.y = y;
        ClampVelocity();
    }

    public void AddY(float y)
    {
        this.velocity.y += y;
        ClampVelocity();
    }

    public bool IsFalling()
    {
        return this.velocity.y < 0f;
    }

    public bool IsGrounded()
    {
        if (characterController != null)
        {
            var radius = characterController.radius;
            var position = transform.position;
            position.y += -characterController.height / 2f + characterController.radius - characterController.skinWidth;
            return Physics.CheckBox(position, radius * Vector3.one, transform.rotation,
                                    groundFilter, QueryTriggerInteraction.Ignore)
            || IsOnPlattform();
        }
        else
        {
            return false;
        }
    }

    public bool IsOnPlattform()
    {
        if (characterController != null)
        {
            var radius = characterController.radius;
            var position = transform.position;
            position.y += -characterController.height / 2f + characterController.radius - characterController.skinWidth;
            return Physics.CheckBox(position, radius * Vector3.one, transform.rotation,
                                    platformFilter, QueryTriggerInteraction.Ignore);
        }
        else
        {
            return false;
        }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
        EnableGravity();
    }

    public void DisableGravity()
    {
        this.gravityDisabled = true;
    }

    public void EnableGravity()
    {
        this.gravityDisabled = false;
    }

    public void Jump(float jumpHeight)
    {
        if (inputLocked)
        {
            return;
        }
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        previouslyGrounded = false;
    }

    public void LockInputForSeconds(float duration)
    {
        inputLocked = true;
        lockedTimer = duration;
    }
}
