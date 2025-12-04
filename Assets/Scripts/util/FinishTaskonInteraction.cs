using UnityEngine;

public class FinishTaskonInteraction : MonoBehaviour, InteractableAction
{
    public int afterN = 1;
    private int counter = 0;

    public SimpleCondition condition;

    void InteractableAction.StartInteraction()
    {
        counter = 0;
    }

    bool InteractableAction.Execute()
    {
        counter++;
        return counter == afterN;
    }

    void InteractableAction.EndInteraction()
    {
        condition.MarkDone();
    }

}
