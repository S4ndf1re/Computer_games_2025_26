
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
    /// Use GetInstance over direct task access, because it could be that the task actually implements a singleton pattern
    /// </summary>
    /// <returns></returns>
    public TaskCondition GetInstance();
}