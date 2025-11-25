using UnityEngine;
using System.Collections.Generic;

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

    private List<Player> currentlyActive;
    private List<Player> collidedPlayers;
    private bool isInFixedUpdateCycle = false;


    void Start()
    {
        currentlyActive = new List<Player>();
        collidedPlayers = new List<Player>();
    }


    void FixedUpdate() {
        isInFixedUpdateCycle = true;
    }

    void Update()
    {
        if(!isInFixedUpdateCycle) {
            return;
        }

        List<Player> toRemove = new List<Player>();
        foreach (var player in currentlyActive)
        {
            if (!collidedPlayers.Contains(player))
            {
                toRemove.Add(player);
            }
        }

        collidedPlayers.Clear();

        foreach (var player in toRemove)
        {
            currentlyActive.Remove(player);
        }

        // Only disable, when we actually removed any players
        if (currentlyActive.Count == 0 && toRemove.Count > 0)
        {
            disableEvent?.Invoke();
        }

        isInFixedUpdateCycle = false;
    }

    void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if (!currentlyActive.Contains(player))
            {
                if (currentlyActive.Count == 0)
                {
                    enableEvent?.Invoke();
                }

                currentlyActive.Add(player);
            }

            collidedPlayers.Add(player);
        }
    }
}
