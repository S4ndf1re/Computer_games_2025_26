using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public float interactRange = 1.5f;
    public InteractionController player;
    public InteractableAction action;

    private Outline outline;
    private bool isHighlighted = false;

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

        // Variable Aktion suchen
        action = GetComponent<InteractableAction>();
    }

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        var cam = Camera.main.GetComponent<CameraController>();

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
            if (player.currentInteractable == this)
            {
                player.currentInteractable = null;

                if (cam.currentTarget == this.transform)
                    cam.ResetCamera();
            }

            if (isHighlighted)
            {
                outline.enabled = false;
                isHighlighted = false;
            }
        }
    }

    public void InvokeInteraction()
    {
        StartCoroutine(FlashOutline());

        // Fokus der Kamera
        var cam = Camera.main.GetComponent<CameraController>();
        if (cam.currentTarget != this.transform)
            cam.FocusOn(transform);

        // Aktion ausführen — wenn vorhanden
        action?.Execute();
    }

    private IEnumerator FlashOutline()
    {
        outline.OutlineColor = Color.gray;
        outline.OutlineWidth = 2f;

        yield return new WaitForSeconds(0.15f);

        outline.OutlineColor = originalColor;
        outline.OutlineWidth = originalWidth;
    }
}
