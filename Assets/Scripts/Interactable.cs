using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactRange = 1.5f;

    public InteractionController player;

    void Start()
    {
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);

        // Wenn in Reichweite → setzen
        if (dist <= interactRange)
        {
            player.currentInteractable = this;
        }
        else if (player.currentInteractable == this)
        {
            player.currentInteractable = null;
        }
    }

    public void InvokeInteraction()
    {
        Debug.Log("Pressed E!");
    }
}
