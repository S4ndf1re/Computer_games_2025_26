using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Das Menü-Panel oder Canvas, das deaktiviert werden soll")]
    public GameObject menuPanel;

    public void CloseMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f; // setzt das Spiel wieder fort (falls du Pausenmodus nutzt)
        }
    }

    public void OpenMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0f; // pausiert das Spiel
        }
    }
}
