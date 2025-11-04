using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyBehaviorNoNav : MonoBehaviour
{
    private TickloopAddable tickloopAddable;
    public TargetTestNoNav targetBehavior;
    public GameObject target;
    public float attentionRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isIdling;
    public bool isMoving;
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
        isIdling = true;
        isMoving = false;
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

        targetBehavior.Move(gameObject, target);

        isMoving = true;
    }
}
