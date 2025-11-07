using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    private TickloopAddable tickloopAddable;
    private NavMeshAgent agent;

    public bool isMovingForward;
    public bool isMovingBackwards;

    public enum State
    {
        
        Forwards,
        ForwardsFinished,
        Backwards,
        BackwardsFinished

    }
    public State currentState;


    void Start()
    {
        tickloopAddable = GetComponent<TickloopAddable>();
        tickloopAddable.triggeredByTickloop += Move;
        agent = GetComponent<NavMeshAgent>();
        currentState = State.BackwardsFinished;
    }

    void Update()
    {
        // if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        if ((agent.destination - agent.transform.position).magnitude < 0.1f || agent.velocity.magnitude < 0.01f)
        {
            switch (currentState){
                case State.Forwards:
                    currentState = State.ForwardsFinished;
                    break;
                case State.Backwards:
                    currentState = State.BackwardsFinished;
                    break;
            }   
        }
    }
    
    void Move(Tickloop tp)
    {
        switch (currentState){
            case State.BackwardsFinished:
                agent.SetDestination(new Vector3(transform.position.x - 2, transform.position.y, transform.position.z));
                currentState = State.Forwards;
                break;
            case State.ForwardsFinished:
                agent.SetDestination(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z));
                currentState = State.Backwards;
                break;
        }
    }
}
