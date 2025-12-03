using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapTrigger : MonoBehaviour, InteractableAction
{

    public string sceneName;
    public LoadSceneMode loadSceneMode;

    [Header("Fade In options")]
    public Image fadeOutImage;
    [Tooltip("The time between each step in seconds")]
    public float speed = 0.1f;

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
        if (fadeOutImage != null)
        {
            Debug.Log("Setting Color");
            var color = fadeOutImage.color;
            color.a = 1;
            fadeOutImage.color = color;
        }

        yield return SceneManager.LoadSceneAsync(sceneName);

        Camera.main.gameObject.SetActive(false);
        var scene = SceneManager.GetSceneByName(sceneName);
        Debug.Log("Swapping Scene");
        SceneManager.SetActiveScene(scene);

        yield return StartCoroutine(FadeInImage());
    }

    IEnumerator FadeInImage()
    {
        for (var i = 255; i >= 0; i--)
        {
            if (fadeOutImage != null)
            {
                var color = fadeOutImage.color;
                color.a = (float)i / 255.0f;
                fadeOutImage.color = color;

                yield return new WaitForSeconds(speed);
            }
        }

    }

}
