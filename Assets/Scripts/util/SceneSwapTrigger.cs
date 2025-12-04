using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapTrigger : MonoBehaviour, InteractableAction
{

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

    public bool Execute()
    {
        if (sceneSwap == null)
        {
            sceneSwap = StartCoroutine(LoadScene());
        }

        return true;
    }

    IEnumerator LoadScene()
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync(sceneName);

        var scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

    }

}
