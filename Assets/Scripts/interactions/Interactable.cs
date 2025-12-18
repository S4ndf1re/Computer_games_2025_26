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
    public float blockForSeconds = 0f;
    private bool hasTriggeredAfterEnter = false;
    private bool blocked = false;
    private Coroutine blockedCoroutine;


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
            if (triggerOnEnter && !hasTriggeredAfterEnter || triggerOnEnter && explicitMultiUse && !blocked)
            {
                InvokeInteraction();
                hasTriggeredAfterEnter = true;
                if (blockedCoroutine != null)
                {
                    StopCoroutine(blockedCoroutine);
                }
                blockedCoroutine = StartCoroutine(BlockOnEnter());
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
        if (oneTimeUse && AllFinished() || AllFinished() && hasTriggeredAfterEnter)
        {
            return;
        }
        else if ((!oneTimeUse) && AllFinished())
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


    IEnumerator BlockOnEnter()
    {
        blocked = true;
        yield return new WaitForSeconds(blockForSeconds);

        blocked = false;
    }
}
