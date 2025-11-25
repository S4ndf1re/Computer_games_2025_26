using UnityEngine;

    /// <summary>
    /// Class <c>MoveJump</c> defines a simple Jump in direction of a target. Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveJump : EnemyAct
{
    public CharacterController enemy;
    public float jumpGravity;
    public float oldVelocityGravity;
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
        velocity = GetComponentInParent<Velocity>();
    }



    public override bool Move()
    {
        
        currentMoveDuration += Time.deltaTime;
        //velocity.y += gravity * Time.deltaTime;

        //Vector3 finalMove = (currentMoveDirection * currentMoveSpeed) + (velocity.y * Vector3.up);
        //enemy.Move(finalMove * Time.deltaTime);

        //jump ends when we land
        // if (groundCheck.IsGrounded(enemy) && velocity.y < 0)
        // {
        //     velocity.y = 0f;
        //     return true;
        // }

        Vector3 move = currentMoveDirection * currentMoveSpeed;
        velocity.SetInstant(move);

        //jump ends when we land
        if (velocity.IsGrounded() && velocity.velocity.y < 0)
        {
            velocity.gravity = oldVelocityGravity;
            return true;
        }
        return false;
        
    }

    public override bool PrepareMove(GameObject target, float currentGravity)
    {
        //only prepare when enemy is grounded
        if (velocity.IsGrounded())
        {
            oldVelocityGravity = currentGravity;
            velocity.gravity = jumpGravity;
            //velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            currentMoveDuration = 0;
            currentMoveDirection = DetermineJumpDirection(target);
            currentMoveSpeed = DetermineJumpSpeed(target);
            velocity.Jump(jumpHeight);
            //orientate the model into direction the enemy is moving
            float targetAngle = Mathf.Atan2(currentMoveDirection.x, currentMoveDirection.z) * Mathf.Rad2Deg;
            enemyModel.rotation = Quaternion.Euler(0, targetAngle, 0);
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
        Debug.Log(totalJumpDuration);
        return currentDistance < maxJumpDistance? currentDistance / totalJumpDuration : maxJumpDistance / totalJumpDuration;
    }

    private float DetermineJumpDuration()
    {
        //freefall formula: h-1/2 * g * t^2 rearranged to t = sqrt(4*h/g) -> -g because our acceleration is negative
        // 2 times because we jump up first and fall then, it is a parabola
        return 2*Mathf.Sqrt(2*jumpHeight/-jumpGravity);
    }

    public override void OnHit(Hitbox hitbox)
    {
        currentMoveDirection = (enemy.transform.position - hitbox.transform.position).normalized;
    }


}
