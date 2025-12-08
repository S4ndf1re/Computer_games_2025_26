using UnityEngine;

public class AddTaskOnInteraction : MonoBehaviour, InteractableAction
{
    public int afterN = 1;
    private int counter = 0;

    public string taskDescription = "";
    public GameObject condition;
    private TaskCondition conditionInterface;
    private bool taskAdded = false;

    void Start()
    {
        conditionInterface = condition.GetComponent<TaskCondition>();
        if (conditionInterface == null)
        {
            Debug.Log("Expected condition to inherit the interface TaskCondition, deactivating");
            enabled = false;
        }

        taskAdded = false;
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
        if (!taskAdded)
        {
            TaskListManager.Instance.SpawnTask(taskDescription, conditionInterface);
            taskAdded = true;
        }
    }

}
