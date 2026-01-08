using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    public GameState state;


    [Header("Lobby Settings")]
    public Transform afterTutorialSpawnpoint;
    public Transform afterStorageRoomSpawnpoint;
    public Transform afterElevatorSpawnpoint;
    public Transform afterTreppenhausSpawnpoint;
    public Interactable afterTutorialDialog;
    public Interactable storageRoomDoor;
    public Interactable staircaseDoor;
    public Interactable printerDialogue;
    public CharacterController player;



    void Awake()
    {
        if (state.currentScene == null)
        {
            state.Clear();
        }

        // Buffer here, since both will get overwritten by the SetSceneBasedOnString call
        var oldScene = state.currentScene;
        var wasElevator = state.wasElevator;
        state.SetSceneBasedOnString(SceneManager.GetActiveScene().name.ToLower());


        switch (state.currentScene)
        {
            case GameState.Scenes.Tutorial:
                SetupTutorial(oldScene);
                break;
            case GameState.Scenes.Lobby:
                SetupLobby(oldScene, wasElevator);
                break;
        }

    }

    void SetupTutorial(GameState.Scenes? oldScene)
    {
        // Nothing to do here
    }

    void SetupLobby(GameState.Scenes? oldScene, bool wasElevator)
    {
        player.enabled = false;
        if (oldScene == GameState.Scenes.Tutorial)
        {
            afterTutorialDialog.enabled = true;
            player.transform.position = afterTutorialSpawnpoint.position;
            player.transform.localScale = afterTutorialSpawnpoint.localScale;
            player.transform.rotation = afterTutorialSpawnpoint.rotation;
        }
        else if (oldScene == GameState.Scenes.StorageRoom)
        {
            afterTutorialDialog.enabled = false;
            staircaseDoor.enabled = true;
            storageRoomDoor.enabled = true;
            printerDialogue.enabled = false;
            player.transform.position = afterStorageRoomSpawnpoint.position;
            player.transform.localScale = afterStorageRoomSpawnpoint.localScale;
            player.transform.rotation = afterStorageRoomSpawnpoint.rotation;
        }
        else if (oldScene == GameState.Scenes.Treppenhaus && !wasElevator)
        {
            afterTutorialDialog.enabled = false;
            staircaseDoor.enabled = true;
            storageRoomDoor.enabled = true;
            printerDialogue.enabled = false;
            player.transform.position = afterTreppenhausSpawnpoint.position;
            player.transform.localScale = afterTreppenhausSpawnpoint.localScale;
            player.transform.rotation = afterTreppenhausSpawnpoint.rotation;
        } else if(oldScene == GameState.Scenes.Treppenhaus && wasElevator)
        {
            afterTutorialDialog.enabled = false;
            staircaseDoor.enabled = true;
            storageRoomDoor.enabled = true;
            printerDialogue.enabled = true;
            player.transform.position = afterElevatorSpawnpoint.position;
            player.transform.localScale = afterElevatorSpawnpoint.localScale;
            player.transform.rotation = afterElevatorSpawnpoint.rotation;
        }
        player.enabled = true;
    }

}
