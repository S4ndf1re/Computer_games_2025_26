using UnityEngine;

public class ActivateCanvasAfterInteraction : MonoBehaviour, InteractableAction
{
    public Canvas canvas;

    public bool Execute() {
        canvas.enabled = true;
        return false;
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
