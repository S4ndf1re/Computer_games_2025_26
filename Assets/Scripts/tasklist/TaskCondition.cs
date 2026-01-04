
public interface TaskCondition
{
    /// <summary>
    /// Check if the task is finished
    /// </summary>
    /// <returns></returns>
    public bool TaskFinished();

    /// <summary>
    /// Can be used to externally trigger a task. Must not be implemented and supported
    /// </summary>
    public void FinishTask();

    public void Reset();

    public void Activate();

    public void Deactivate();

    public bool IsActive();

    /// <summary>
    /// SetTask can be implemented to support the receiving of the related task.
    /// This method is called the moment the task is instantiated.
    /// The implementor is responsible to save the received task if the task should get manipulated at a later point in time
    /// </summary>
    /// <param name="relatedTask"></param>
    public void SetTask(Task relatedTask)
    {

    }

    /// <summary>
    /// Use GetInstance over direct task access, because it could be that the task actually implements a singleton pattern
    /// </summary>
    /// <returns></returns>
    public TaskCondition GetInstance();
}