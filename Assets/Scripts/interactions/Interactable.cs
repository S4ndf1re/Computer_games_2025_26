using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool oneTimeUse = false;

    [Tooltip("Tigger once player enters")]
    public bool triggerOnEnter = false;
    public bool explicitMultiUse = false;
    private bool hasTriggeredAfterEnter = false;


    public Outline setOutline;

    public float interactRange = 1.5f;
    public InteractionController player;

    private InteractableAction[] actions;
    private Dictionary<InteractableAction, bool> hasInteracted;
    private Dictionary<InteractableAction, bool> hasFinished;

    private Outline outline;

    private Color originalColor;
    private float originalWidth;

    void Start()
    {
        if (setOutline != null)
        {
            outline = setOutline;
        }
        else
        {
            outline = GetComponent<Outline>();
        }
        // if (outline == null)
        //     outline = gameObject.AddComponent<Outline>();

        if (outline != null)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 5f;

            originalColor = outline.OutlineColor;
            originalWidth = outline.OutlineWidth;
            outline.enabled = false;
        }

        hasInteracted = new Dictionary<InteractableAction, bool>();
        hasFinished = new Dictionary<InteractableAction, bool>();
        actions = GetComponents<InteractableAction>();
        foreach (var action in actions)
        {
            hasInteracted[action] = false;
            hasFinished[action] = false;
        }

        hasTriggeredAfterEnter = false;

    }

    bool AllInteracted()
    {
        foreach (var action in hasInteracted)
        {
            if (!action.Value)
            {
                return false;
            }
        }
        return true;
    }

    bool AllFinished()
    {
        foreach (var action in hasFinished)
        {
            if (!action.Value)
            {
                return false;
            }
        }
        return true;
    }


    void Update()
    {

        // Falls One-Time und bereits benutzt → dauerhaft grau, keine Highlight-Logik mehr
        if (oneTimeUse && AllFinished())
        {
            if (outline != null)
            {
                outline.enabled = true;
                outline.OutlineColor = Color.gray;
                outline.OutlineWidth = 2f;
            }
            return;
        }

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= interactRange)
        {
            player.currentInteractable = this;
            if (triggerOnEnter && !hasTriggeredAfterEnter || triggerOnEnter && explicitMultiUse)
            {
                InvokeInteraction();
                hasTriggeredAfterEnter = true;
            }

            if (outline != null)
            {
                outline.enabled = true;
            }
        }
        else
        {
            if (outline != null)
            {
                outline.enabled = false;
            }

            if (player.currentInteractable == this)
            {
                player.currentInteractable = null;
            }
        }
    }

    public void InvokeInteraction()
    {
        // Wenn One-Time und bereits interagiert → abbrechen
        if (oneTimeUse && AllFinished() || AllFinished() && hasTriggeredAfterEnter && !explicitMultiUse)
        {
            return;
        }
        else if (!oneTimeUse && AllFinished())
        {
            foreach (var action in actions)
            {
                hasFinished[action] = false;
                hasInteracted[action] = false;
            }
        }

        StartCoroutine(FlashOutline());

        if (actions != null)
        {
            foreach (var a in actions)
            {
                if (!hasInteracted[a])
                {
                    a.StartInteraction();
                    hasInteracted[a] = true;
                }
                if (!hasFinished[a])
                {
                    var finished = a.Execute();
                    hasFinished[a] = finished;

                    if (finished)
                    {
                        a.EndInteraction();
                    }
                }
            }
        }
    }

    private IEnumerator FlashOutline()
    {
        if (outline != null)
        {
            outline.enabled = true;

            outline.OutlineColor = Color.gray;
            outline.OutlineWidth = 2f;


            // Wenn One-Time → dauerhaft grau
            if (oneTimeUse)
            {
                outline.OutlineColor = Color.gray;
                outline.OutlineWidth = 2f;
            }
            else
            {
                // sonst originaler Style
                outline.OutlineColor = originalColor;
                outline.OutlineWidth = originalWidth;
            }
        }

        yield return new WaitForSeconds(0.15f);
    }
}
