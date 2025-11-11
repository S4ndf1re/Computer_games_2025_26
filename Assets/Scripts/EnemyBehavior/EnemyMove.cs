using System.Collections;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    public float walkDistance;
    public float walkSpeed;
    public float jumpHeight;
    public float jumpDistance;
    public float jumpSpeed;
    public float moveDuration;

    protected float playerSpeed = 5.0f;
    //protected float jumpHeight = 1.5f;
    protected float gravityValue = -35f;

    protected CharacterController controller;
    protected Vector3 playerVelocity;
    protected bool groundedPlayer;

    public enum State
    {
        waitingForWalk,
        walking,
        waitingForJump,
        jumping
    }
    public State nextState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        controller = transform.parent.gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void Move(GameObject enemy, GameObject target);

    protected Vector3 DetermineWalkPoint(GameObject enemy, GameObject target)
    {
        return enemy.transform.position + walkDistance * (target.transform.position - enemy.transform.position).normalized;
    }
    
    protected Vector3 DetermineWalkPoint(GameObject enemy, Vector3 target)
    {
        return enemy.transform.position + walkDistance * (target - enemy.transform.position).normalized;
    }

    protected Vector3 DetermineJumpPoint(GameObject enemy, GameObject target)
    {
        return enemy.transform.position + jumpDistance * (target.transform.position - enemy.transform.position).normalized;
    }
    protected Vector3 DetermineJumpPoint(GameObject enemy, Vector3 target)
    {
        return enemy.transform.position + jumpDistance * (target - enemy.transform.position).normalized;
    }

    protected IEnumerator JumpCoroutine(GameObject enemy, Vector3 end)
    {
        Vector3 start = enemy.transform.position;
        Collider collider = enemy.GetComponent<Collider>();
        Debug.Log(collider.bounds);
        float totalDuration = jumpDistance / jumpSpeed;
        float passedTime = 0;

        //make sure the end is on ground or the agent teleports
        if (Physics.Raycast(end + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f))
        {
            end.y = hit.point.y + collider.bounds.extents.y;
        }
        while (passedTime < totalDuration)
        {

            passedTime += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(start, end, passedTime / totalDuration);
            //x = passedTime / totalDuration 
            //f(0,5) = a * x * (1-x) * jumpHeight = jumpheight -> a = 4
            float x = passedTime / totalDuration;
            currentPosition.y += 4 * x * (1 - x) * jumpHeight;
            enemy.transform.position = currentPosition;
            yield return null;
        }
        enemy.transform.position = end;
        while (!Physics.Raycast(enemy.transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit groundHit, 0.5f))
        {

            passedTime += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(start, end, passedTime / totalDuration);
            //x = passedTime / totalDuration 
            //f(0,5) = a * x * (1-x) * jumpHeight = jumpheight -> a = 4
            float x = passedTime / totalDuration;
            currentPosition.y += 4 * x * (1 - x) * jumpHeight;
            enemy.transform.position = currentPosition;
            yield return null;
        }

    }
    
    protected IEnumerator WalkCoroutine(GameObject enemy, Vector3 end)
    {
        Vector3 start = enemy.transform.position;
        Collider collider = enemy.GetComponent<Collider>();
        Debug.Log(collider.bounds);
        float totalDuration = walkDistance / walkSpeed;
        float passedTime = 0;

        //make sure the end is on ground or the agent teleports
        if (Physics.Raycast(end + Vector3.up * 1.5f, Vector3.down, out RaycastHit hit, 2f))
        {
            end.y = hit.point.y + collider.bounds.extents.y;
        }
        if  (Mathf.Abs(end.y - enemy.transform.position.y) < 0.5f)
        {
            while (passedTime < totalDuration)
        {
            passedTime += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(start, end, passedTime / totalDuration);
            enemy.transform.position = currentPosition;
            yield return null;
        }
        enemy.transform.position = end; 
        } 

    }
}
