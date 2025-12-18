using UnityEngine;
using System.Collections.Generic;

public class HideOnInteract : MonoBehaviour, InteractableAction
{

    public GameState state;
    public int afterN = 1;
    private int counter = 0;

    [Header("Parent whose children will be hidden")]
    public Transform targetParent;

    private List<Renderer> renderers = new List<Renderer>();

    void InteractableAction.StartInteraction()
    {
        counter = 0;

        if (targetParent == null)
        {
            Debug.LogError("InstantHideOnTrigger: Kein Parent gesetzt!", this);
            enabled = false;
            return;
        }

        // alle Renderer im gesamten Hierarchiebaum sammeln
        renderers.AddRange(targetParent.GetComponentsInChildren<Renderer>());

        if (state.hasInk == true)
        {
            // sofort unsichtbar machen
            foreach (Renderer r in renderers)
            {
                r.enabled = false;
            }
        }
    }

    public bool Execute()
    {
        if (counter == afterN)
        {
            return true;
        }

        counter++;
        return false;
    }

    void InteractableAction.EndInteraction()
    {
        // sofort unsichtbar machen
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }
}
