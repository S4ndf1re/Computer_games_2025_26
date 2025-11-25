using System.Collections.Generic;
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
    private Vector3 lastPosition;

    List<Collider> objectsOnPlatform = new List<Collider>();
    List<Collider> collidedObjects = new List<Collider>();

    private bool isInFixedUpdateCycle = false;

    void Start()
    {
        tickloopAddable = GetComponent<TickloopAddable>();
        tickloopAddable.triggeredByTickloop += MoveTowardsNextWaypoint;
    }


    void FixedUpdate()
    {
        isInFixedUpdateCycle = true;

        elapsedTime += Time.fixedDeltaTime;
        if (isMoving)
        {
            float timePercentage = elapsedTime / timeToNextWaypoint;
            GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, timePercentage));

            // Vector3 positiondelta = transform.position - lastPosition;
            // foreach (var collider in objectsOnPlatform)
            // {
            //     var deltaV = Vector3.Scale(new Vector3(1.0f, 1.0f, 1.0f), positiondelta);
            //     collider.GetComponent<Velocity>()?.AddInstant(positiondelta);
            //     // var position = collider.transform.position;
            //     // position.x += positiondelta.x;
            //     // position.z += positiondelta.z;
            //     // collider.transform.position = position;
            // }
            // lastPosition = transform.position;
            if (timePercentage >= 1)
            {
                isMoving = false;
            }
        } else {

        }
    }

    void Update()
    {
        if (isInFixedUpdateCycle)
        {
            List<Collider> toRemove = new List<Collider>();
            foreach (var collider in objectsOnPlatform)
            {
                if (!collidedObjects.Contains(collider))
                {
                    toRemove.Add(collider);
                }
            }

            collidedObjects.Clear();

            foreach (var collider in toRemove)
            {
                Debug.Log("objectsOnPlatform.Remove(collider);");
                objectsOnPlatform.Remove(collider);
                collider.transform.SetParent(null);
                // collider.GetComponent<Velocity>()?.EnableGravity();
            }
            isInFixedUpdateCycle = false;
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


    void OnTriggerStay(Collider other)
    {
        if (!objectsOnPlatform.Contains(other))
        {
            Debug.Log("objectsOnPlatform.Add(other);");
            objectsOnPlatform.Add(other);
            other.transform.SetParent(this.transform);
            // other.GetComponent<Velocity>()?.DisableGravity();
        }

        collidedObjects.Add(other);
    }
}
