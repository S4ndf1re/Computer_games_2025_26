using UnityEngine;

public class SetElevatorFlag : MonoBehaviour, InteractableAction
{

    public GameState state;

    bool InteractableAction.Execute()
    {
        state.wasElevator = true;
        return true;
    }

    bool InteractableAction.IsActive()
    {
        return this.enabled;
    }
}
