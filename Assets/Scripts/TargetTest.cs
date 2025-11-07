using System;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TargetTest : MonoBehaviour
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
    public void Move(NavMeshAgent agent, GameObject target)
    {
        switch (nextState)
        {
            case State.walk:
                nextState = State.jump;
                agent.SetDestination(DetermineWalkPoint(agent, target));
                break;
            case State.jump:
                Vector3 end = DetermineJumpPoint(agent, target);
                StartCoroutine(JumpCoroutine(agent, end));
                nextState = State.walk;
                //agent.SetDestination(DetermineJumpPoint(agent, target));
                break;
        }
    }

    private Vector3 DetermineWalkPoint(NavMeshAgent agent, GameObject target)
    {
        return agent.transform.position + walkDistance * (target.transform.position - agent.transform.position).normalized;
    }

    private Vector3 DetermineJumpPoint(NavMeshAgent agent, GameObject target)
    {
        return agent.transform.position + jumpDistance * (target.transform.position - agent.transform.position).normalized;
    }

    IEnumerator JumpCoroutine(NavMeshAgent agent, Vector3 end)
    {
        agent.enabled = false;
        Vector3 start = agent.transform.position;
        Collider collider = agent.GetComponent<Collider>();
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
            agent.transform.position = currentPosition;
            yield return null;
        }
        agent.Warp(end);
        agent.enabled = true;
    }
}
