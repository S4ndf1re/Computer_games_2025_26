using System.Collections;
using UnityEngine;

public class TimedCondition : MonoBehaviour, TaskCondition
{

    public float timeInSeconds = 0f;
    public bool isDone = false;
    private Coroutine current;


    void OnEnable()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        Debug.Log("Starting Timer on Enable");
        current = StartCoroutine(StartTime());
    }

    void OnDisable()
    {
        if (current != null)
        {
            Debug.Log("Stopping");
            StopCoroutine(current);
        }
    }

    public void Activate()
    {
        if (current != null)
        {
            Debug.Log("Stopping");
            StopCoroutine(current);
        }
        enabled = true;
    }

    public void Deactivate()
    {
        if (current != null)
        {
            Debug.Log("Stopping");
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

}
