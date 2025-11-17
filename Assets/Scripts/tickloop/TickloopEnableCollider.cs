using UnityEngine;

/// <summary>
/// The TickloopEnableCollider will send enable and disable events based on the corresponding collider zone.
/// This Component requires any type of collider with the trigger set as a co-component within the same game object.
/// The colliding game object must have any collider, and a player. One the player enters the "zone",
/// TickloopAddables can decide wheather to register to the tickloop.
/// When the players leave, the Addables will get notified, so that they can remove themselfes from the tickloop
/// </summary>
public class TickloopEnableCollider : MonoBehaviour
{

    public delegate void Enable();
    public event Enable enableEvent;

        public delegate void Disable();
        public event Enable disableEvent;


    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() != null) {
            Debug.Log("Registered Player Entering in Bounding box " + name);
            enableEvent?.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.GetComponent<Player>() != null) {
            Debug.Log("Registered Player Leaving in Bounding box " + name);
            disableEvent?.Invoke();
        }
    }
}
