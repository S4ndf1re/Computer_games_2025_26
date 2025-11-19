using UnityEngine;

    /// <summary>
    /// Class <c>MoveIdle</c> defines an idle behavior of an enemy in which the enemy is pulled towards the ground by gravity and knocked back when taking damage.
    /// Can be used to control an enemy by adding it to its EnemyMoveHandler.
    /// </summary>
public class MoveIdle : EnemyAct
{
    public CharacterController enemy;
    private Vector3 playerVelocity;
    public float gravity;
    public float maxWalkDistance;
    public float maxWalkSpeed;
    public float currentMoveDuration;
    public Vector3 currentMoveDirection;
    public EnemyGroundCheck groundCheck;
    public bool wasHit = false;


    void Start()
    {
        groundCheck = GetComponentInParent<EnemyGroundCheck>();
        enemy = GetComponentInParent<CharacterController>();
    }
    public override bool Move()
    {
        if (wasHit)
        {
            currentMoveDuration += Time.deltaTime;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        Vector3 finalMove = (currentMoveDirection * maxWalkSpeed) + (playerVelocity.y * Vector3.up);
        enemy.Move(finalMove * Time.deltaTime);
        if (wasHit && currentMoveDuration >= maxWalkDistance / maxWalkSpeed && groundCheck.isGrounded(enemy))
        {
            ResetToIdle();
            return true;
        }
        return false;
    }

    public override void OnHit(Hitbox hitbox)
    {
        wasHit = true;
        currentMoveDuration = 0;
        currentMoveDirection = (enemy.transform.position - hitbox.transform.position).normalized;
    }

    public override bool PrepareMove(GameObject target, float currentGravity)
    {
        gravity = currentGravity;
        ResetToIdle();
        return true;
    }

    private void ResetToIdle()
    {
        wasHit = false;
        currentMoveDuration = 0;
        currentMoveDirection = Vector3.zero;
    }

}
