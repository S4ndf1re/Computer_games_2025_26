using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public Interactable currentInteractable;

    // Wird vom PlayerInput über UnityEvent getriggert
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("Trying to interact");
        if (!ctx.performed)
            return;

        // Sonst Interactable-Action starten
        if (currentInteractable != null)
        {
            Debug.Log("Interacting");
            currentInteractable.InvokeInteraction();
        }
    }
}
