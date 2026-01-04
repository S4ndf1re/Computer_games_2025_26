using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Das Menü-Panel oder Canvas, das deaktiviert werden soll")]
    public GameObject menuPanel;

    public void CloseMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void OpenMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
