using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TaskListSpawner : MonoBehaviour
{
    public Task prefabTask;

    void Update()
    {
        var rect = GetComponent<RectTransform>();
        if (rect != null)
            LayoutRebuilder.MarkLayoutForRebuild(rect);
    }

    /// <summary>
    /// Spawn a single task. The condition will determine if the task is ready to be marked as finished
    /// </summary>
    /// <param name="taskDescription"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public Task SpawnTask(string taskDescription, TaskCondition condition)
    {
        var obj = Instantiate(prefabTask, gameObject.transform);
        obj.condition = condition;
        StartCoroutine(obj.TypeText(taskDescription));
        return obj;
    }
}
