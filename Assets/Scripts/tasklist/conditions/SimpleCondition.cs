using UnityEngine;

public class SimpleCondition : MonoBehaviour, TaskCondition
{
    public bool isDone = false;

    bool TaskCondition.TaskFinished()
    {
        return isDone;
    }

    TaskCondition TaskCondition.GetInstance()
    {
        return this;
    }

    void TaskCondition.FinishTask()
    {
        isDone = true;
    }

}
