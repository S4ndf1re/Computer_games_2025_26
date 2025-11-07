using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class TargetTestNoNav : MonoBehaviour
{

    public float walkDistance;
    public float jumpHeight;
    public float jumpDistance;
    public float jumpSpeed;
    public enum State
    {
        beforeWalk,
        walk,
        beforeJump,
        jump
    }
    public State nextState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextState = State.walk;
    }

    // Update is called once per frame
    void Update()
    {
        // if ((agent.destination - agent.transform.position).magnitude < 0.1f || agent.velocity.magnitude < 0.01f)
        // {
        //     switch (nextState){
        //         case State.Forwards:
        //             nextState = State.ForwardsFinished;
        //             break;
        //         case State.Backwards:
        //             nextState = State.BackwardsFinished;
        //             break;
        //     }   
        // }
    }
    public void Move(GameObject agent, GameObject target)
    {
        Debug.Log("called");
        Vector3 end = DetermineJumpPoint(agent, target);
        Debug.Log(end);
        StartCoroutine(JumpCoroutine(agent, end));
        //agent.SetDestination(DetermineJumpPoint(agent, target));

    }

    private Vector3 DetermineWalkPoint(GameObject agent, GameObject target)
    {
        return agent.transform.position + walkDistance * (target.transform.position - agent.transform.position).normalized;
    }

    private Vector3 DetermineJumpPoint(GameObject agent, GameObject target)
    {
        return agent.transform.position + jumpDistance * (target.transform.position - agent.transform.position).normalized;
    }

    IEnumerator JumpCoroutine(GameObject agent, Vector3 end)
    {
        Debug.Log("gestaret");
        Collider collider = agent.GetComponent<Collider>();
        if (Physics.Raycast(end + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f))
        {
            end.y = hit.point.y + collider.bounds.extents.y;
        }
        Vector3 start = agent.transform.position;
        float totalDuration = jumpDistance / jumpSpeed;
        float passedTime = 0;
        while (passedTime < totalDuration)
        {
            passedTime += Time.deltaTime;
            Vector3 currentPosition = Vector3.Lerp(start, end, passedTime / totalDuration);
            //x = passedTime / totalDuration 
            //f(0,5) = a * x * (1-x) * jumpHeight = jumpheight -> a = 4
            float x = passedTime / totalDuration;
            currentPosition.y += 4 * x * (1 - x) * jumpHeight;
            agent.transform.position = currentPosition;
            yield return null;
        }
        agent.transform.position = end;
    }
}
