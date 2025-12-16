using UnityEngine;

public class PaperTaskCondition : MonoBehaviour, TaskCondition
{

    public static PaperTaskCondition condition;
    public bool isDone = false;
    public GameState state;


    void Awake()
    {
        if (condition != null && condition != this)
        {
            Destroy(gameObject);
        }
        else
        {
            isDone = false;
            condition = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    void Update()
    {
        if (state.hasPaper)
        {
            this.isDone = true;
        }
    }


    TaskCondition TaskCondition.GetInstance()
    {
        return condition;
    }

    bool TaskCondition.TaskFinished()
    {
        return isDone;
    }

    void TaskCondition.FinishTask()
    {
        isDone = true;
    }
}
