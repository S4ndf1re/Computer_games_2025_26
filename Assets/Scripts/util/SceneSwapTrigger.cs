using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapTrigger : MonoBehaviour, InteractableAction
{

    public string sceneName;
    public LoadSceneMode loadSceneMode;

    public void Execute()
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }

}
