using UnityEngine;
using UnityEngine.AI;

public class TargetNotTest : MonoBehaviour
{

    enum State
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Move(NavMeshAgent agent, GameObject target)
    {
        Debug.Log("moveIdle");
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            agent.SetDestination(new Vector3(agent.transform.position.x - 2, agent.transform.position.y, agent.transform.position.z));
        }
        else if (direction == 1)
        {
            agent.SetDestination(new Vector3(agent.transform.position.x + 2, agent.transform.position.y, agent.transform.position.z));
        }


    }
}
