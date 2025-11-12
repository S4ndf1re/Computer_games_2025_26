using UnityEngine;
using UnityEngine.UIElements;

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
        Vector3 finalMove = (currentMoveDirection * maxWalkSpeed) + (playerVelocity.y * Vector3.up);
        enemy.Move(finalMove * Time.deltaTime);
        if (currentMoveDuration >= maxWalkSpeed / currentMoveDistance && enemy.isGrounded)
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
            currentMoveDistance = DetermineWalkDistance(enemy, target);
            return true;
        }
        return false;

    }

    private Vector3 DetermineWalkDirection(CharacterController enemy, GameObject target)
    {
        return (target.transform.position - enemy.transform.position).normalized;
    }

    private float DetermineWalkDistance(CharacterController enemy, GameObject target)
    {
        float currentDistance = (target.transform.position - enemy.transform.position).magnitude;
        return currentDistance < maxWalkDistance ? currentDistance : maxWalkDistance;
    }
}
