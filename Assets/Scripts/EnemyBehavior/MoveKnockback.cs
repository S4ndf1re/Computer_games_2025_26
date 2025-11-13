using UnityEngine;

    /// <summary>
    /// Class <c>MoveKnockback</c> defines a simple Knockback away from a target. Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveKnockback : EnemyAct
{
    public CharacterController enemy;
    private Vector3 playerVelocity;
    public float gravity;
    public float maxWalkDistance;
    public float maxWalkSpeed;
    public float currentMoveDuration;
    private Vector3 currentMoveDirection;
    private float currentMoveDistance;
    public override bool Move()
    {
        currentMoveDuration += Time.deltaTime;
        playerVelocity.y += gravity * Time.deltaTime;
        Vector3 finalMove = (currentMoveDirection * maxWalkSpeed) + (playerVelocity.y * Vector3.up);
        enemy.Move(finalMove * Time.deltaTime);
        if (currentMoveDuration >= currentMoveDistance / maxWalkSpeed && enemy.isGrounded)
        {
            return true;
        }
        return false;
    }

    public override bool PrepareMove(CharacterController enemy, GameObject target, float currentGravity)
    {
        if (enemy.isGrounded)
        {
            gravity = currentGravity;
            this.enemy = enemy;
            currentMoveDuration = 0;
            currentMoveDirection = DetermineWalkDirection(enemy, target);
            currentMoveDistance = maxWalkDistance;
            return true;
        }
        return false;

    }

    private Vector3 DetermineWalkDirection(CharacterController enemy, GameObject target)
    {
        return (enemy.transform.position - target.transform.position).normalized;
    }


    public override void OnHit(Hitbox hitbox)
    {
        currentMoveDirection = (enemy.transform.position - hitbox.transform.position).normalized;
    }
}
