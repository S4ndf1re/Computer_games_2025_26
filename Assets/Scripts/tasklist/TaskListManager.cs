using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// For the TaskListManager, it is important that this will outlive all scenes. For this reason, the manager is marked as DontDestronOnLoad
/// This will however lead to duplicate instances, when developing scenes. Hence it is important, to actually use TaskListManager.Instance
/// As the actual instance of the TaskListManager
/// </summary>
public class TaskListManager : MonoBehaviour
{
    /// <summary>
    /// The actual current instance of the task List
    /// </summary>
    public static TaskListManager Instance;

    public TaskListSpawner spawner;
    private List<Task> tasks;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            tasks = new List<Task>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    void Update()
    {
        var toRemove = new List<int>();

        for (var i = 0; i < tasks.Count; i++)
        {
            var task = tasks[i];
            if (task.condition == null || task.condition.GetInstance().TaskFinished())
            {
                toRemove.Add(i);
            }
        }

        foreach (var idx in toRemove)
        {
            tasks[idx].TriggerDespawn();
            tasks.RemoveAt(idx);
        }
    }

    /// <summary>
    /// Spawn a single task. The condition will determine when the task should get removed again
    /// </summary>
    /// <param name="taskDescription"></param>
    /// <param name="condition"></param>
    public void SpawnTask(string taskDescription, TaskCondition condition)
    {
        if (condition != null)
        {
            tasks.Add(spawner.SpawnTask(taskDescription, condition));
        }
    }
}
