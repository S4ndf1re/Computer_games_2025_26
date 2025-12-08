using GLTFast.Schema;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{

    public enum Scenes
    {
        Tutorial,
        Lobby,
        StorageRoom
    }

    public Scenes currentScene = Scenes.Tutorial;

    public void SetSceneBasedOnString(string sceneName)
    {
        if (sceneName == "lobby")
        {
            this.currentScene = Scenes.Lobby;
        } else if (sceneName == "storageroom")
        {
            this.currentScene = Scenes.StorageRoom;
        } else if(sceneName == "tutorial")
        {
            this.currentScene = Scenes.Tutorial;
        }
    }
}
