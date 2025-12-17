using UnityEngine;

public class InkTaskCondition : MonoBehaviour, TaskCondition
{

    public static InkTaskCondition condition;
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
        if (state.hasInk)
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
