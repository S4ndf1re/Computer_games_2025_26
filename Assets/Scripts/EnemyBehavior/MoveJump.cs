using UnityEngine;

public class MoveJump : EnemyAct
{
    public CharacterController enemy;
    private Vector3 playerVelocity;
    public float gravity;
    public float jumpHeight;
    public float maxJumpDistance;
    public float maxJumpSpeed;
    public float currentMoveDuration;
    private Vector3 currentMoveDirection;
    private float currentMoveSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool Move()
    {
        currentMoveDuration += Time.deltaTime;
        playerVelocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = (currentMoveDirection * currentMoveSpeed) + (playerVelocity.y * Vector3.up);
        enemy.Move(finalMove * Time.deltaTime);


        if (enemy.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            return true;
        }
        return false;
    }

    public override bool PrepareMove(CharacterController enemy, GameObject target, float currentGravity)
    {

        if (enemy.isGrounded)
        {
            this.enemy = enemy;
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

    protected float DetermineJumpDistance(GameObject enemy, GameObject target)
    {
        float currentDistance = (target.transform.position - enemy.transform.position).magnitude;
        return currentDistance < maxJumpDistance ? currentDistance : maxJumpDistance;
    }
    
    protected float DetermineJumpSpeed(GameObject enemy, GameObject target)
    {
        float currentDistance = (target.transform.position - enemy.transform.position).magnitude;
        return currentDistance < maxJumpDistance? currentDistance/maxJumpDistance * maxJumpSpeed : maxJumpSpeed;
    }
}
