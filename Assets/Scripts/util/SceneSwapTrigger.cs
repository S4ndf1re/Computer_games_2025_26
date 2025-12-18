using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapTrigger : MonoBehaviour, InteractableAction
{
    public int afterN = 0;
    private int counter = 0;

    public string sceneName;
    public LoadSceneMode loadSceneMode;
    public Animator transition;

    private Coroutine sceneSwap;

    void Start()
    {
        if (sceneSwap != null)
            StopCoroutine(sceneSwap);
        sceneSwap = null;
    }

    void InteractableAction.StartInteraction()
    {
        counter = 0;
    }

    public bool Execute()
    {
        if (counter >= afterN)
        {
            if (sceneSwap == null)
            {
                sceneSwap = StartCoroutine(LoadScene());
            }
            return true;
        }
        counter++;

        return false;
    }

    IEnumerator LoadScene()
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync(sceneName);

        var scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }

}
