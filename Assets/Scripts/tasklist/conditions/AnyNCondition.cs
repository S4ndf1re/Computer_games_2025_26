using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnyNCondition : MonoBehaviour, TaskCondition
{

    public List<GameObject> conditions;
    private List<TaskCondition> conditionsIntern = new List<TaskCondition>();
    public bool isFinished = false;
    public int minFinished = 1;
    private Task task;
    private int oldCounter = -1;

    void Awake()
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

        var counter = 0;
        foreach (var cond in conditionsIntern)
        {
            if (cond.GetInstance().TaskFinished())
            {
                counter += 1;
            }
        }
        isFinished = counter >= minFinished && counter > 0;

        return isFinished;
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
        task = relatedTask;
        foreach (var cond in conditionsIntern)
        {
            cond.SetTask(relatedTask);
        }
    }

    void Update()
    {

        var counter = 0;
        foreach (var cond in conditionsIntern)
        {
            if (cond.GetInstance().TaskFinished())
            {
                counter += 1;
            }
        }

        if (task != null && oldCounter != counter)
        {
            StringBuilder builder = new StringBuilder(task.originalTaskText);
            builder.Replace("$", "" + counter + "/" + minFinished);
            task.ShowTextInstant(builder.ToString());
            oldCounter = counter;
        }

    }

}
