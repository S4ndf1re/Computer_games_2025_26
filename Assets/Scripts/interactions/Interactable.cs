using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool oneTimeUse = false;     // NEU: Im Inspector auswählbar

    public float interactRange = 1.5f;
    public InteractionController player;

    private InteractableAction[] actions;

    private Outline outline;
    private bool isHighlighted = false;
    private bool hasInteracted = false;

    private Color originalColor;
    private float originalWidth;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline == null)
            outline = gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.white;
        outline.OutlineWidth = 5f;

        originalColor = outline.OutlineColor;
        originalWidth = outline.OutlineWidth;
        outline.enabled = false;

        actions = GetComponents<InteractableAction>();
    }

    void Update()
    {
        // Falls One-Time und bereits benutzt → dauerhaft grau, keine Highlight-Logik mehr
        if (oneTimeUse && hasInteracted)
        {
            outline.enabled = true;
            outline.OutlineColor = Color.gray;
            outline.OutlineWidth = 2f;
            return;
        }

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= interactRange)
        {
            player.currentInteractable = this;

            if (!isHighlighted)
            {
                outline.enabled = true;
                isHighlighted = true;
            }
        }
        else
        {
            if (isHighlighted)
            {
                outline.enabled = false;
                isHighlighted = false;
            }
        }
    }

    public void InvokeInteraction()
    {
        // Wenn One-Time und bereits interagiert → abbrechen
        if (oneTimeUse && hasInteracted)
            return;

        // Markiere Interaktion als erfolgt
        hasInteracted = true;

        StartCoroutine(FlashOutline());

        if (actions != null)
        {
            foreach (var a in actions)
                a.Execute();
        }
    }

    private IEnumerator FlashOutline()
    {
        outline.enabled = true;

        outline.OutlineColor = Color.gray;
        outline.OutlineWidth = 2f;

        yield return new WaitForSeconds(0.15f);

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
}
