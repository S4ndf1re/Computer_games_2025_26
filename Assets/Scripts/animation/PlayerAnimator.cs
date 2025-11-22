using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    /// ---------------------------
    /// Serializable Fields
    /// ---------------------------
    public Animator animator;

    public Velocity velocity;
    public float AnimationAcceleration = 10.0f;

    /// ---------------------------
    /// Internal Variables
    /// ---------------------------
    
    // Animator IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    //private int animIDMotionSpeed;
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

    void Update()
    {
        if (velocity.GetVelocity().magnitude > 0)
        {
            animationBlend = Mathf.Lerp(animationBlend, velocity.GetVelocity().magnitude, Time.deltaTime * AnimationAcceleration);
            if (animationBlend < 0.01f) animationBlend = 0f;
            animator.SetFloat(animIDSpeed, animationBlend);
        }

        Debug.Log(velocity.IsGrounded());
        if (velocity.IsGrounded())
        {
            animator.SetBool(animIDJump, false);
            animator.SetBool(animIDFreeFall, false);
        } else
        {
            animator.SetBool(animIDFreeFall, true);
        }

    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        //animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
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
        animator.SetTrigger("Dash");
    }
}
