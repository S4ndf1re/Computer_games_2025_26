using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskConditionSceneSwap : MonoBehaviour, TaskCondition
{
    public bool done = false;
    public string targetScene = "";
    public float delayAfterSceneEnter = 2;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name == targetScene)
        {
            StartCoroutine(TriggerAfterDelay(delayAfterSceneEnter));
        };
    }

    bool TaskCondition.TaskFinished()
    {
        return done;
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
}
