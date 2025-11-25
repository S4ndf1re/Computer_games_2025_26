using UnityEngine;

    /// <summary>
    /// Class <c>MoveWalk</c> defines a simple Walk in direction of a target. Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveWalk : EnemyAct
{
    public CharacterController enemy;
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
        velocity = GetComponentInParent<Velocity>();
    }

    public override bool Move()
    {
        currentMoveDuration += Time.deltaTime;
        //velocity.y += gravity * Time.deltaTime;
        //Vector3 finalMove = (currentMoveDirection * maxWalkSpeed) + (velocity.y * Vector3.up);
        //enemy.Move(finalMove * Time.deltaTime);
        Vector3 move = (currentMoveDirection * maxWalkSpeed) ;
        velocity.SetInstant(move);



        //end if moved far enough and grounded
        if (currentMoveDuration >= currentMoveDistance / maxWalkSpeed && velocity.IsGrounded())
        {
            return true;
        }
        return false;
    }

    public override bool PrepareMove(GameObject target, float currentGravity)
    {
        //only perpare if enemy is grounded
        if (velocity.IsGrounded())
        {
            gravity = currentGravity;
            currentMoveDuration = 0;
            currentMoveDirection = DetermineWalkDirection(enemy, target);
            currentMoveDistance = DetermineWalkDistance(enemy, target);
            //orientate the model into direction the enemy is moving
            float targetAngle = Mathf.Atan2(currentMoveDirection.x, currentMoveDirection.z) * Mathf.Rad2Deg;
            enemyModel.rotation = Quaternion.Euler(0, targetAngle, 0);
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
