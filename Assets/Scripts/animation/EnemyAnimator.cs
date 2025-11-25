using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    /// ---------------------------
    /// Serializable Fields
    /// ---------------------------
    public Animator animator;
    public EnemyMoveHandler moveHandler;
    public float AnimationAcceleration = 10.0f;

    public Velocity velocity;

    /// ---------------------------
    /// Internal Variables
    /// ---------------------------

    // Animator IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;

    private float animationBlend;


    void Start()
    {
        AssignAnimationIDs();
    }

    void Update()
    {
        Vector3 currentVelocity = velocity.velocity;
        float horizontalSpeed = new Vector2(currentVelocity.x, currentVelocity.z).magnitude;

        animationBlend = Mathf.Lerp(animationBlend, horizontalSpeed, Time.deltaTime * AnimationAcceleration);
        if (animationBlend < 0.01f) animationBlend = 0f;

        animator.SetFloat(animIDMotionSpeed, 1);
        animator.SetFloat(animIDSpeed, animationBlend);

        if (velocity.IsGrounded())
        {
            animator.SetBool(animIDJump, false);
            animator.SetBool(animIDFreeFall, false);
            animator.SetBool(animIDGrounded, true);
        } else
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
}
