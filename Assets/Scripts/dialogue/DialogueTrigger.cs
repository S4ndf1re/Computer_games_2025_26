using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour, InteractableAction
{
    public List<string> dialogueText;
    public GameObject dialoguePrefab;
    public VelocityPlayerController velocityController;

    private DialogueController dialogue;
    private CameraController cam;
    private int dialogueIndex;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
        var obj = Instantiate(dialoguePrefab);
        obj.SetActive(false);

        dialogueIndex = 0;

        Debug.Log(dialogueText.Count);

        dialogue = obj.GetComponent<DialogueController>();
        dialogue.followTarget = transform;
    }

    public bool Execute()
    {
        Debug.Log("Execute interaction");
        if (dialogueIndex == dialogueText.Count) {
            return true;
        }

        // update text
        dialogue.ShowText(dialogueText[dialogueIndex]);
        dialogueIndex++;
        Debug.Log(dialogueIndex);

        // Always return false. the actual check is done at the top, because once the last dialog was shown, the player must interact one more time to disable the dialog
        return false;
    }

    public void StartInteraction()
    {
        Debug.Log("Start Interaction");
        cam.FocusOn(transform);
        if (velocityController != null)
            velocityController.canMove = false;
        dialogue.gameObject.SetActive(true);
        dialogueIndex = 0;
    }

    public void EndInteraction()
    {
        Debug.Log("End Interaction");
        cam.ResetCamera();
        if (velocityController != null)
            velocityController.canMove = true;
        dialogue.gameObject.SetActive(false);
    }
}
