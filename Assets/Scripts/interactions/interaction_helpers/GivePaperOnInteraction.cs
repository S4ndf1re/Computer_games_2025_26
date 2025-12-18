using UnityEngine;

public class GivePaperOnInteraction : MonoBehaviour, InteractableAction
{

    public GameState state;
    public int afterN = 1;
    private int counter = 0;


    void InteractableAction.StartInteraction()
    {
        counter = 0;
    }

    public bool Execute()
    {
        if (counter == afterN)
        {
            return true;
        }

        counter++;
        return false;
    }

    void InteractableAction.EndInteraction()
    {
        state.hasPaper = true;
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
