using System.Collections.Generic;
using UnityEngine;

public class ConveyorOnTick : MonoBehaviour
{
    public Vector3 velocity;

    public List<Velocity> currentlyActive = new List<Velocity>();


    void OnEnable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
    }


    void OnTriggerEnter(Collider other)
    {
        var velocityController = other.GetComponent<Velocity>();
        if (velocityController != null)
        {
            Debug.Log("Player found");
            currentlyActive.Add(velocityController);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player removed");
        var velocityController = other.GetComponent<Velocity>();
        if (velocityController != null)
        {
            currentlyActive.Remove(velocityController);
        }
    }

    void Trigger(Tickloop loop, int nth_trigger)
    {
        foreach (var velo in currentlyActive)
        {
            velo.AddInstant(velocity);
        }

    }
}
