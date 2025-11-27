using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public float interactRange = 1.5f;
    public InteractionController player;
    public InteractableAction action;

    public VelocityPlayerController velocityController;

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
            // Nur dieses Interactable verliert Fokus, nicht andere
            if (player.currentInteractable == this)
            {
                player.currentInteractable = null;

                // Wenn Kamera gerade dieses Objekt fokussiert → Reset
                if (cam.currentTarget == this.transform)
                {
                    cam.ResetCamera();

                    // Movement wieder erlauben
                    if (velocityController != null)
                        velocityController.canMove = true;
                }
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

        var cam = Camera.main.GetComponent<CameraController>();

        if (!cam.isFocused)
        {
            cam.FocusOn(transform);

            if (velocityController != null)
                velocityController.canMove = false;

            action?.Execute(); // optional: Dialog etc.
            return;
        }

        if (cam.isFocused && cam.currentTarget == this.transform)
        {
            cam.ResetCamera();

            if (velocityController != null)
                velocityController.canMove = true;

            return;
        }
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
