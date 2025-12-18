using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskConditionSceneSwap : MonoBehaviour, TaskCondition
{
    public bool done = false;
    public string targetScene = "";
    public float delayAfterSceneEnter = 2;
    private Coroutine current;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

        var scene = SceneManager.GetActiveScene();
        if (scene.name == targetScene)
        {
            current = StartCoroutine(TriggerAfterDelay(delayAfterSceneEnter));
        }
    }

    bool TaskCondition.TaskFinished()
    {
        return done;
    }

    void TaskCondition.FinishTask()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        done = true;
    }

    TaskCondition TaskCondition.GetInstance()
    {
        return this;
    }


    private IEnumerator TriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        done = true;
    }

    void TaskCondition.Activate()
    {
        enabled = true;
    }

    void TaskCondition.Deactivate()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        enabled = false;
    }

    bool TaskCondition.IsActive()
    {
        return enabled;
    }

    void TaskCondition.Reset()
    {
        if (current != null)
        {
            StopCoroutine(current);
        }
        done = false;
    }
}
