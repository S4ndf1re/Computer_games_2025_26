using System;
using System.Collections;
using System.Text;
using UnityEngine;

public class TimedCondition : MonoBehaviour, TaskCondition
{

    public float timeInSeconds = 0f;
    public bool isDone = false;
    private Coroutine current;
    private float currentElapsedTime = 0f;
    private Task task;


    void OnEnable()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        current = StartCoroutine(StartTime());
    }

    void OnDisable()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
    }

    public void Activate()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        enabled = true;
    }

    public void Deactivate()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        enabled = false;
    }

    public TaskCondition GetInstance()
    {
        return this;
    }

    public bool TaskFinished()
    {
        return this.isDone;
    }

    IEnumerator StartTime()
    {
        if (isDone)
        {
            yield break;
        }

        Debug.Log("Running Timer");
        currentElapsedTime = 0;
        yield return new WaitForSeconds(timeInSeconds);
        isDone = true;
    }

    bool TaskCondition.IsActive()
    {
        return enabled;
    }


    void TaskCondition.Reset()
    {
        if (current != null)
        {
            Debug.Log("Stopping");
            StopCoroutine(current);
        }
        this.isDone = false;
    }

    void TaskCondition.FinishTask()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        isDone = true;
    }

    void TaskCondition.SetTask(Task relatedTask)
    {
        task = relatedTask;
    }

    void FixedUpdate()
    {
        if (task != null)
        {
            StringBuilder builder = new StringBuilder(task.originalTaskText);
            builder.Replace("$", (int)(timeInSeconds - currentElapsedTime) + "");
            task.ShowTextInstant(builder.ToString());
        }
        currentElapsedTime += Time.fixedDeltaTime;
    }

}
