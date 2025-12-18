using UnityEngine;

public class FinishTaskonInteraction : MonoBehaviour, InteractableAction
{
    public int afterN = 1;
    private int counter = 0;

    public GameObject condition;
    private TaskCondition conditionInterface;

    void Start()
    {
        conditionInterface = condition.GetComponent<TaskCondition>();
        if (conditionInterface == null)
        {
            Debug.Log("Exptected condition to implement TaskCondition. Despawning gameobject");
            Destroy(gameObject);
        }
    }

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
        conditionInterface.GetInstance().FinishTask();
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }

}
