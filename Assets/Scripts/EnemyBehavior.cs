using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private TickloopAddable tickloopAddable;
    private NavMeshAgent agent;
    public TargetNotTest idleBehavior;
    public TargetTest targetBehavior;
    public GameObject target;
    public float attentionRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isIdling;
    
    void Start()
    {
        tickloopAddable = GetComponent<TickloopAddable>();
        tickloopAddable.triggeredByTickloop += Move;
        agent = GetComponent<NavMeshAgent>();
        isIdling = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if ((agent.destination - agent.transform.position).magnitude < 0.1f || agent.velocity.magnitude < 0.01f)
        // {
        //     isMoving = false;
        // }
    }
    void Move(Tickloop tp)
    {
        if ((target.transform.position - agent.transform.position).magnitude < attentionRadius)
            isIdling = false;
        else
            isIdling = true;

        if (isIdling)
            idleBehavior.Move(agent, target);
        else
            targetBehavior.Move(agent, target);
    }
}
