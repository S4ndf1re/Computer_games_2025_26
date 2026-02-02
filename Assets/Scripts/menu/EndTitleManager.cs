using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndTitleManager : MonoBehaviour
{
    [Header("General Menu Panel")]
    [SerializeField] public GameObject menuPanel;

    public TMP_Text timeText;
    public TMP_Text deathsText;

    public bool activeOnStart = false;

    private float time;
    private int deaths;

    void Start()
    {
        if (menuPanel != null && !activeOnStart)
            menuPanel.SetActive(false);
        
        if (StatTracker.Instance != null) {
            StatTracker.Instance.RegisterDialogueEnd();
            time = StatTracker.Instance.playTime;
            deaths = StatTracker.Instance.deaths;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formatted = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

        timeText.SetText(formatted);
        deathsText.SetText(deaths.ToString());
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
