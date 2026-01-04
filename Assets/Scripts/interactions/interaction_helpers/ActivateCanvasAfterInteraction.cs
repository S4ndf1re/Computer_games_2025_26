using UnityEngine;

public class ActivateCanvasAfterInteraction : MonoBehaviour, InteractableAction
{
    public Canvas canvas;
    public GameObject metronome;

    public bool Execute() {
        canvas.enabled = true;
        metronome.SetActive(true);
        return false;
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
