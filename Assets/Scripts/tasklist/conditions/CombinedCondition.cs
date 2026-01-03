using System.Collections.Generic;
using UnityEngine;

public class CombinedCondition : MonoBehaviour, TaskCondition
{

    public List<GameObject> conditions;
    private List<TaskCondition> conditionsIntern;
    public bool isFinished = false;


    void Start()
    {
        foreach (var cond in conditions)
        {
            var taskCond = cond.GetComponent<TaskCondition>();
            if (taskCond != null)
            {
                conditionsIntern.Add(taskCond);
            }
        }
    }

    void TaskCondition.Activate()
    {
        foreach (var cond in conditionsIntern)
        {
            cond.GetInstance().Activate();
        }
        enabled = true;
    }

    void TaskCondition.Deactivate()
    {
        foreach (var cond in conditionsIntern)
        {
            cond.GetInstance().Deactivate();
        }
        enabled = false;
    }

    bool TaskCondition.IsActive()
    {
        return enabled;
    }

    public TaskCondition GetInstance()
    {
        return this;
    }

    void TaskCondition.Reset()
    {
        foreach (var cond in conditionsIntern)
        {
            cond.GetInstance().Reset();
        }
        isFinished = false;
    }

    public bool TaskFinished()
    {
        if (isFinished)
        {
            return true;
        }
        foreach (var cond in conditionsIntern)
        {
            if (!cond.GetInstance().TaskFinished())
            {
                return false;
            }
        }

        isFinished = true;
        return true;
    }

    void TaskCondition.FinishTask()
    {
        isFinished = true;
        foreach (var cond in conditionsIntern)
        {
            cond.GetInstance().FinishTask();
        }
    }

    void TaskCondition.SetTask(Task relatedTask)
    {
        foreach (var cond in conditionsIntern)
        {
            cond.SetTask(relatedTask);
        }
    }
}
