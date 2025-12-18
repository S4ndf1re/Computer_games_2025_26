using UnityEngine;

public class ActivateAfterInteraction : MonoBehaviour, InteractableAction
{
    public GameObject toActivate;

    public bool Execute()
    {
        toActivate.SetActive(true);
        return true;
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
