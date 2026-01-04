using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("General Menu Panel")]
    [SerializeField] public GameObject menuPanel;
    [SerializeField] public GameObject settingsPanel;

    private bool isPaused = false;

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (menuPanel == null) {
            Debug.Log("MenuPanel is null");
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (menuPanel == null) {
            Debug.Log("MenuPanel is null");
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Spiel wird beendet...");
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void Settings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
}
