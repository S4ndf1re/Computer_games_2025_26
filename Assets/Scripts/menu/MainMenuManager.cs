using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("General Menu Panel")]
    [SerializeField] public GameObject menuPanel;

    public bool activeOnStart = false;
    private bool isPaused = false;

    void Start()
    {
        if (menuPanel != null && !activeOnStart)
            menuPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Debug.Log("Spiel wird beendet...");
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
