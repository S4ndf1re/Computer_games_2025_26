using UnityEngine;

    /// <summary>
    /// Class <c>MoveJump</c> defines a simple Jump in direction of a target. Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveJump : EnemyAct
{
    public CharacterController enemy;
    private Vector3 playerVelocity;
    public float gravity;
    public float jumpHeight;
    public float maxJumpDistance;
    public float maxJumpSpeed;
    public float currentMoveDuration;
    public Vector3 currentMoveDirection;
    private float currentMoveSpeed;
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
            currentMoveDirection = DetermineWalkDirection(enemy.gameObject, target);
            currentMoveSpeed = DetermineJumpSpeed(enemy.gameObject, target);
            return true;
        }
        return false;
    }
    
    protected Vector3 DetermineWalkDirection(GameObject enemy, GameObject target)
    {
        return (target.transform.position - enemy.transform.position).normalized;
    }
    /// <summary>
    /// Determines the jumpspeed so that the enemy lands on the targets position even when the maxJumpdistance is bigger than the actual distance.
    /// </summary>
    protected float DetermineJumpSpeed(GameObject enemy, GameObject target)
    {
        float currentDistance = (target.transform.position - enemy.transform.position).magnitude;
        return currentDistance < maxJumpDistance? currentDistance/maxJumpDistance * maxJumpSpeed : maxJumpSpeed;
    }

    public override void OnHit(Hitbox hitbox)
    {
        currentMoveDirection = (enemy.transform.position - hitbox.transform.position).normalized;
    }


}
