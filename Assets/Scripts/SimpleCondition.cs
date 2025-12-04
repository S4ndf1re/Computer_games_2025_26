using UnityEngine;

public class SimpleCondition : MonoBehaviour, TaskCondition
{
    public bool isDone = false;

    bool TaskCondition.TaskFinished()
    {
        return isDone;
    }

    public void MarkDone()
    {
        isDone = true;
    }
}
