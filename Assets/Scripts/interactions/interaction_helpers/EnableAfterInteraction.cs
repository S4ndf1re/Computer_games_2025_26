using UnityEngine;

public class EnableAfterInteraction : MonoBehaviour, InteractableAction
{
    public Interactable toEnable;

    public bool Execute()
    {
        toEnable.enabled = true;

        return true;
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
