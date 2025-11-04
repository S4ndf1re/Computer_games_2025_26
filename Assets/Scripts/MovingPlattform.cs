using UnityEngine;

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

    void Start()
    {
        tickloopAddable = GetComponent<TickloopAddable>();
        tickloopAddable.triggeredByTickloop += MoveTowardsNextWaypoint;
    }

    void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        if (isMoving)
        {
            float timePercentage = elapsedTime / timeToNextWaypoint;
            transform.position = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, timePercentage);
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
}
