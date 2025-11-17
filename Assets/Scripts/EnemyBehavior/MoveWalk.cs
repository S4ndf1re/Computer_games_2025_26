using UnityEngine;

    /// <summary>
    /// Class <c>MoveWalk</c> defines a simple Walk in direction of a target. Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveWalk : EnemyAct
{
    public CharacterController enemy;
    private Vector3 playerVelocity;
    public float gravity;
    public float maxWalkDistance;
    public float maxWalkSpeed;
    public float currentMoveDuration;
    private Vector3 currentMoveDirection;
    private float currentMoveDistance;
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
        Vector3 finalMove = (currentMoveDirection * maxWalkSpeed) + (playerVelocity.y * Vector3.up);
        enemy.Move(finalMove * Time.deltaTime);
        if (currentMoveDuration >= currentMoveDistance / maxWalkSpeed && groundCheck.isGrounded(enemy))
        {
            return true;
        }
        return false;
    }

    public override bool PrepareMove(CharacterController enemy, GameObject target, float currentGravity)
    {
        if (groundCheck.isGrounded(enemy))
        {
            gravity = currentGravity;
            this.enemy = enemy;
            currentMoveDuration = 0;
            currentMoveDirection = DetermineWalkDirection(enemy, target);
            currentMoveDistance = DetermineWalkDistance(enemy, target);
            return true;
        }
        return false;

    }

    private Vector3 DetermineWalkDirection(CharacterController enemy, GameObject target)
    {
        return (target.transform.position - enemy.transform.position).normalized;
    }

    /// <summary>
    /// Determines the walkDistance so that the enemy lands on the targets position even when the maxWalkDistance is bigger than the actual distance.
    /// </summary>
    private float DetermineWalkDistance(CharacterController enemy, GameObject target)
    {
        float currentDistance = (target.transform.position - enemy.transform.position).magnitude;
        return currentDistance < maxWalkDistance ? currentDistance : maxWalkDistance;
    }

    public override void OnHit(Hitbox hitbox)
    {
        currentMoveDirection = (enemy.transform.position - hitbox.transform.position).normalized;
    }
}
