using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    /// ---------------------------
    /// Serializable Fields
    /// ---------------------------
    public Animator animator;
    public Velocity velocity;
    public VelocityPlayerController velocityPlayerController;
    public float AnimationAcceleration = 10.0f;

    /// ---------------------------
    /// Internal Variables
    /// ---------------------------

    // Animator IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;
    //private int animIDPunch;
    //private int animIDPush;
    //private int animIDCrawl;

    private float animationBlend;

    //Movement Bools
    private bool isMoving;
    private bool isDashing;
    private bool IsFalling;

    void Start()
    {
        AssignAnimationIDs();
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = velocity.velocity;
        float horizontalSpeed = new Vector2(currentVelocity.x, currentVelocity.z).magnitude;

        animationBlend = Mathf.Lerp(animationBlend, horizontalSpeed, Time.deltaTime * AnimationAcceleration);
        if (animationBlend < 0.01f) animationBlend = 0f;

        animator.SetFloat(animIDMotionSpeed, 1);
        animator.SetFloat(animIDSpeed, animationBlend);

        if (velocity.isGrounded)
        {
            animator.SetBool(animIDJump, false);
            animator.SetBool(animIDFreeFall, false);
            animator.SetBool(animIDGrounded, true);
        }
        else
        {
            animator.SetBool(animIDGrounded, false);
            animator.SetBool(animIDFreeFall, true);
        }

    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        //animIDPunch = Animator.StringToHash("Punch");
        //animIDPush = Animator.StringToHash("Push");
        //animIDCrawl = Animator.StringToHash("Crawl");
    }

    /// ---------------------------
    /// Input Callbacks
    /// ---------------------------

    public void OnJump()
    {
        animator.SetBool(animIDJump, true);
    }

    public void OnDash()
    {
        //animator.SetTrigger("Dash");
    }

    public void OnLand()
    {
        // Do nothing
    }

    public void OnFootstep()
    {
        // Do nothing
    }
}
