using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    public GameState state;


    [Header("Lobby Settings")]
    public Transform afterTutorialSpawnpoint;
    public Transform afterStorageRoomSpawnpoint;
    public Interactable afterTutorialDialog;
    public CharacterController player;



    void Start()
    {
        if (state.currentScene == null)
        {
            state.Clear();
        }

        var oldScene = state.currentScene;
        state.SetSceneBasedOnString(SceneManager.GetActiveScene().name.ToLower());


        switch (state.currentScene)
        {
            case GameState.Scenes.Tutorial:
                SetupTutorial(oldScene);
                break;
            case GameState.Scenes.Lobby:
                SetupLobby(oldScene);
                break;
            case GameState.Scenes.StorageRoom:
                break;
        }

    }

    void SetupTutorial(GameState.Scenes? oldScene)
    {
        // Nothing to do here
    }

    void SetupLobby(GameState.Scenes? oldScene)
    {
        if (oldScene == GameState.Scenes.Tutorial)
        {
            afterTutorialDialog.enabled = true;
            player.enabled = false;
            player.transform.position = afterTutorialSpawnpoint.position;
            player.transform.localScale = afterTutorialSpawnpoint.localScale;
            player.transform.rotation = afterTutorialSpawnpoint.rotation;
            player.enabled = true;
        }
        else if (oldScene == GameState.Scenes.StorageRoom)
        {
            afterTutorialDialog.enabled = false;
            player.enabled = false;
            player.transform.position = afterStorageRoomSpawnpoint.position;
            player.transform.localScale = afterStorageRoomSpawnpoint.localScale;
            player.transform.rotation = afterStorageRoomSpawnpoint.rotation;
            player.enabled = true;
        }

    }

}
