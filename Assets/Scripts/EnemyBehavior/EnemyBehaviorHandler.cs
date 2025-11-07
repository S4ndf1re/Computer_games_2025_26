using UnityEngine;

public class EnemyBehaviorHandler : MonoBehaviour
{
    public EnemyMove idleBehavior;
    public EnemyMove targetBehavior;

    public GameObject target;
    public float attentionRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isIdling;
    
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Move;
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
        // if ((target.transform.position - gameObject.transform.position).magnitude < attentionRadius
        // && Physics.Raycast(gameObject.transform.position + Vector3.up * 1f, target.transform.position, out RaycastHit hit, attentionRadius)
        // && hit.transform == target.transform)
        if ((target.transform.position - gameObject.transform.position).magnitude < attentionRadius)
        {
            isIdling = false;
        }
        else
        {
            isIdling = true;
        }
        if (isIdling)
            idleBehavior.Move(gameObject, target);
        else
            targetBehavior.Move(gameObject, target);
    }
}
