using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("General Menu Panel")]
    [SerializeField] public GameObject menuPanel;

    public Toggle tickloopToggle;
    public GameState state;
    public bool activeOnStart = false;

    void Start()
    {
        if (menuPanel != null && !activeOnStart)
            menuPanel.SetActive(false);
    }

    public void StartGame()
    {
        state.tickloopEnabled = tickloopToggle.isOn;
        SceneManager.LoadScene("IntroScene");
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
