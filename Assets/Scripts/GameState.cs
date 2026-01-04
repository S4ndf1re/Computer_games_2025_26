using GLTFast.Schema;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{

    public enum Scenes
    {
        Tutorial,
        Lobby,
        StorageRoom,
        Treppenhaus,
        Boss,
    }

    public bool tickloopEnabled = true;

    public Scenes? currentScene = null;
    public bool hasPaper = false;
    public bool hasInk = false;

    public void Clear()
    {
        this.currentScene = null;
        this.hasPaper = false;
        this.hasInk = false;
    }

    public void SetSceneBasedOnString(string sceneName)
    {
        if (sceneName == "lobby")
        {
            this.currentScene = Scenes.Lobby;
        }
        else if (sceneName == "storageroom")
        {
            this.currentScene = Scenes.StorageRoom;
        }
        else if (sceneName == "tutorial")
        {
            this.currentScene = Scenes.Tutorial;
        }
        else if (sceneName == "treppenhaus")
        {
            this.currentScene = Scenes.Treppenhaus;
        }
        else if (sceneName == "boss")
        {
            this.currentScene = Scenes.Boss;
        }
    }
}
