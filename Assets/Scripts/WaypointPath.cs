using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform GetWaypoint(int waypoint)
    {
        return transform.GetChild(waypoint);
    }

    public int GetNextWaypointIndex(int currentWaypoint)
    {
        int nextWaypoint = (currentWaypoint + 1) % transform.childCount;
        return nextWaypoint;
    }
}
