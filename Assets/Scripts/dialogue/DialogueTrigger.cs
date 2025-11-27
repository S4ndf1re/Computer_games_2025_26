using UnityEngine;

public class DialogueTrigger : MonoBehaviour, InteractableAction
{
    public string dialogueText;

    public void Execute()
    {
        Debug.Log("Dialogue started: " + dialogueText);
        // Hier startest du dein Dialogsystem
    }
}
