using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    private TickloopAddable tickloopAddable;
    
    public WaypointPath waypointPath;

    public float speed;
    public float timePerWaypoint;
    public float timeToNextWaypoint;
    private int nextWaypointIndex;

    float elapsedTime;

    private Transform previousWaypoint;
    private Transform nextWaypoint;

    private bool isMoving;
    private Vector3 lastPosition;

    List<Collider> objectsOnPlatform = new List<Collider>();

    void Start()
    {
        tickloopAddable = GetComponent<TickloopAddable>();
        tickloopAddable.triggeredByTickloop += MoveTowardsNextWaypoint;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (isMoving)
        {
            float timePercentage = elapsedTime / timeToNextWaypoint;
            transform.position = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, timePercentage);
            Vector3 positiondelta = transform.position - lastPosition;
            foreach (var collider in objectsOnPlatform)
            {
                collider.enabled = false;
                collider.transform.position += positiondelta;
                collider.enabled = true;
            }
            lastPosition = transform.position;
            if(timePercentage >= 1)
            {
                isMoving = false;
            }
        }
    }
    public void MoveTowardsNextWaypoint(Tickloop tp)
    {
        isMoving = true;
        previousWaypoint = waypointPath.GetWaypoint(nextWaypointIndex);
        nextWaypointIndex = waypointPath.GetNextWaypointIndex(nextWaypointIndex);
        nextWaypoint = waypointPath.GetWaypoint(nextWaypointIndex);
        lastPosition = transform.position;
        elapsedTime = 0;

        float distance = Vector3.Distance(previousWaypoint.position, nextWaypoint.position);
        if (speed != 0)
        {
            timeToNextWaypoint = distance / speed;
        }
        else
        {
            timeToNextWaypoint = timePerWaypoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENter");
        objectsOnPlatform.Add(other);
        //other.transform.SetParent(transform, true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        objectsOnPlatform.Remove(other);
        //other.transform.SetParent(null, true);
    }
}
