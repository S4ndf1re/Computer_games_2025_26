using UnityEngine;

public class Velocity : MonoBehaviour
{

    public Vector3 velocity;
    public float gravity = -9.81f;
    public Vector3 maxGroundVelocity = Vector3.positiveInfinity;
    public Vector3 minGroundVelocity = Vector3.negativeInfinity;

    public Vector3 maxAirVelocity = Vector3.right * 10f + Vector3.forward * 10f + Vector3.up * float.PositiveInfinity;
    public Vector3 minAirVelocity = Vector3.left * 10f + Vector3.back * 10f + Vector3.down * float.PositiveInfinity;


    [Header("Controller")]
    public CharacterController characterController;


    [Header("Debug")]
    public bool isGrounded;
    private bool gravityDisabled = false;
    private bool inputLocked = false;
    private float lockedTimer = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(inputLocked) {
            lockedTimer -= Time.deltaTime;
            if (lockedTimer <= 0f) {
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

        if (IsGrounded())
        {
            ResetVelocity();
            velocity.y = gravity;
        }
        isGrounded = IsGrounded();
    }

    public void AddInstant(Vector3 toAdd)
    {
        if (inputLocked) {
            return;
        }
        this.velocity += toAdd;
        if (IsGrounded())
        {
            this.velocity.x = Mathf.Clamp(this.velocity.x, minGroundVelocity.x, maxGroundVelocity.x);
            this.velocity.y = Mathf.Clamp(this.velocity.y, minGroundVelocity.y, maxGroundVelocity.y);
            this.velocity.z = Mathf.Clamp(this.velocity.z, minGroundVelocity.z, maxGroundVelocity.z);
        }
        else
        {
            this.velocity.x = Mathf.Clamp(this.velocity.x, minAirVelocity.x, maxAirVelocity.x);
            this.velocity.y = Mathf.Clamp(this.velocity.y, minAirVelocity.y, maxAirVelocity.y);
            this.velocity.z = Mathf.Clamp(this.velocity.z, minAirVelocity.z, maxAirVelocity.z);
        }
    }

    /// <summary>
    /// Setting the velocity in u/s, except in the y direction. The y direction is added to apply gravity
    /// </summary>
    public void SetInstant(Vector3 toAdd)
    {
        if (inputLocked) {
            return;
        }
        this.velocity.x = toAdd.x;
        this.velocity.y += toAdd.y;
        this.velocity.z = toAdd.z;
        if (IsGrounded())
        {
            this.velocity.x = Mathf.Clamp(this.velocity.x, minGroundVelocity.x, maxGroundVelocity.x);
            this.velocity.y = Mathf.Clamp(this.velocity.y, minGroundVelocity.y, maxGroundVelocity.y);
            this.velocity.z = Mathf.Clamp(this.velocity.z, minGroundVelocity.z, maxGroundVelocity.z);
        }
        else
        {
            this.velocity.x = Mathf.Clamp(this.velocity.x, minAirVelocity.x, maxAirVelocity.x);
            this.velocity.y = Mathf.Clamp(this.velocity.y, minAirVelocity.y, maxAirVelocity.y);
            this.velocity.z = Mathf.Clamp(this.velocity.z, minAirVelocity.z, maxAirVelocity.z);
        }
    }

    public void SetInstant(Vector3 toAdd, bool withoutY ) {
        if (inputLocked) {
            return;
        }
        this.velocity.x = toAdd.x;
        if (!withoutY) {
            this.velocity.y += toAdd.y;
        }
        this.velocity.z = toAdd.z;
        if (IsGrounded())
        {
            this.velocity.x = Mathf.Clamp(this.velocity.x, minGroundVelocity.x, maxGroundVelocity.x);
            this.velocity.y = Mathf.Clamp(this.velocity.y, minGroundVelocity.y, maxGroundVelocity.y);
            this.velocity.z = Mathf.Clamp(this.velocity.z, minGroundVelocity.z, maxGroundVelocity.z);
        }
        else
        {
            this.velocity.x = Mathf.Clamp(this.velocity.x, minAirVelocity.x, maxAirVelocity.x);
            this.velocity.y = Mathf.Clamp(this.velocity.y, minAirVelocity.y, maxAirVelocity.y);
            this.velocity.z = Mathf.Clamp(this.velocity.z, minAirVelocity.z, maxAirVelocity.z);
        }
    }

    public void SetY(float y)
    {
        if(IsGrounded()) {
            this.velocity.y = Mathf.Clamp(y, minGroundVelocity.y, maxGroundVelocity.y);
        } else {
            this.velocity.y = Mathf.Clamp(y, minAirVelocity.y, maxAirVelocity.y);
        }
    }

    public void AddY(float y)
    {
        if(IsGrounded()) {
            this.velocity.y = Mathf.Clamp(this.velocity.y + y, minGroundVelocity.y, maxGroundVelocity.y);
        } else {
            this.velocity.y = Mathf.Clamp(this.velocity.y + y, minAirVelocity.y, maxAirVelocity.y);
        }
    }

    public bool IsFalling()
    {
        return this.velocity.y < 0f;
    }

    public bool IsGrounded()
    {
        if (characterController != null)
        {
            return characterController.isGrounded;
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
        if (inputLocked) {
            return;
        }
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void LockInputForSeconds(float duration) {
        inputLocked = true;
        lockedTimer = duration;
    }
}
