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
        if (enabled)
            isDone = true;
    }

    void TaskCondition.Activate()
    {
        enabled = true;
    }

    void TaskCondition.Deactivate()
    {
        enabled = false;
    }

    bool TaskCondition.IsActive()
    {
        return enabled;
    }

    void TaskCondition.Reset()
    {
        isDone = false;
    }



}
