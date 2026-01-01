using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatTracker : MonoBehaviour
{
    public static StatTracker Instance;

    // -------- Runtime Stats --------
    public int hits;
    public int damageTaken;
    public int deaths;
    public float playTime;

    private string currentSceneName;
    private float sceneStartTime;
    private bool inDialogue = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        currentSceneName = SceneManager.GetActiveScene().name;
        sceneStartTime = Time.time;
    }

    private void Update()
    {
        if(!inDialogue)
        {
            playTime += Time.deltaTime;
        }
    }

    // -------- Scene Handling --------
    private void OnSceneUnloaded(Scene scene)
    {
        SaveStats(scene.name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetStatsForNewScene(scene.name);
    }

    public void RegisterDialogueStart()
    {
        this.inDialogue = true;
    }

    public void RegisterDialogueEnd()
    {
        this.inDialogue = false;
    }

    public void RegisterHit()
    {
        hits++;
    }

    public void RegisterDamageTaken(int damage)
    {
        damageTaken += damage;
    }

    public void RegisterDeath()
    {
        deaths++;
    }

    // -------- Save Logic --------
    public void SaveStats(string sceneName)
    {
        Stats data = new Stats
        {
            sceneName = sceneName,
            hits = this.hits,
            damageTaken = this.damageTaken,
            deaths = this.deaths,
            playTime = this.playTime
        };

        string json = JsonUtility.ToJson(data, true);

        string path = Path.Combine(
            Application.persistentDataPath,
            $"stats_{sceneName}.json"
        );

        File.WriteAllText(path, json);
        Debug.Log($"Stats saved: {path}");
    }

    private void ResetStatsForNewScene(string newSceneName)
    {
        inDialogue = false;
        currentSceneName = newSceneName;
        sceneStartTime = Time.time;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
