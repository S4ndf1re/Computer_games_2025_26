using UnityEngine;

    /// <summary>
    /// Class <c>MoveJump</c> defines a simple Jump in direction of a target. Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveJump : EnemyAct
{
    public CharacterController enemy;
    public Vector3 playerVelocity;
    public float gravity;
    public float jumpHeight;
    public float maxJumpDistance;
    public float currentMoveDuration;
    public Vector3 currentMoveDirection;
    public float currentMoveSpeed;
    public EnemyGroundCheck groundCheck;


    void Start()
    {
        groundCheck = GetComponentInParent<EnemyGroundCheck>();
        enemy = GetComponentInParent<CharacterController>();
    }



    public override bool Move()
    {
        
        currentMoveDuration += Time.deltaTime;
        playerVelocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = (currentMoveDirection * currentMoveSpeed) + (playerVelocity.y * Vector3.up);
        enemy.Move(finalMove * Time.deltaTime);

        //jump ends when we land
        if (groundCheck.isGrounded(enemy) && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            return true;
        }
        return false;
    }

    public override bool PrepareMove(GameObject target, float currentGravity)
    {
        //only prepare when enemy is grounded
        if (groundCheck.isGrounded(enemy))
        {
            gravity = currentGravity;
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            currentMoveDuration = 0;
            currentMoveDirection = DetermineJumpDirection(target);
            currentMoveSpeed = DetermineJumpSpeed(target);

            return true;
        }
        return false;
    }
    
    private Vector3 DetermineJumpDirection(GameObject target)
    {
        Vector3 enemyWithoutY = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        Vector3 targetWithoutY = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        return (targetWithoutY - enemyWithoutY).normalized;
    }
    /// <summary>
    /// Determines the jumpspeed so that the enemy lands on the targets position even when the maxJumpdistance is bigger than the actual distance.
    /// </summary>
    private float DetermineJumpSpeed(GameObject target)
    {
        Vector3 enemyWithoutY = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        Vector3 targetWithoutY = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        float currentDistance = (targetWithoutY - enemyWithoutY).magnitude;
        float totalJumpDuration = DetermineJumpDuration();
        return currentDistance < maxJumpDistance? currentDistance / totalJumpDuration : maxJumpDistance / totalJumpDuration;
    }

    private float DetermineJumpDuration()
    {
        //freefall formula: h-1/2 * g * t^2 rearranged to t = sqrt(4*h/g) -> -g because our acceleration is negative
        // 2 times because we jump up first and fall then, it is a parabola
        return 2*Mathf.Sqrt(2*jumpHeight/-gravity);
    }

    public override void OnHit(Hitbox hitbox)
    {
        currentMoveDirection = (enemy.transform.position - hitbox.transform.position).normalized;
    }


}
