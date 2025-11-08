using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Das Menü-Panel oder Canvas, das angezeigt werden soll")]
    public GameObject menuPanel;

    private bool isPaused = false;

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false); // Menü beim Start ausblenden
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
        if (menuPanel == null) return;

        menuPanel.SetActive(true);
        Time.timeScale = 0f; // pausiert das Spiel
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (menuPanel == null) return;

        menuPanel.SetActive(false);
        Time.timeScale = 1f; // setzt das Spiel fort
        isPaused = false;
    }
}
