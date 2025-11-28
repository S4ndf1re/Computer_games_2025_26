using UnityEngine;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour, InteractableAction
{
    public List<string> dialogueText;
    public GameObject dialoguePrefab;
    public VelocityPlayerController velocityController;

    private DialogueController dialogue;
    private CameraController cam;
    private bool isActive;
    private int dialogueIndex;
    private int dialogueAmount;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
        var obj = Instantiate(dialoguePrefab);
        obj.SetActive(false);

        dialogueIndex = 0;
        dialogueAmount = dialogueText.Count;

        Debug.Log(dialogueAmount);

        dialogue = obj.GetComponent<DialogueController>();
        dialogue.followTarget = transform;
    }

    public void Execute()
    {
        // initial Activation
        if (!isActive && dialogueAmount != dialogueIndex)
        {
            cam.FocusOn(transform);
            if (velocityController != null)
                velocityController.canMove = false;
            dialogue.gameObject.SetActive(true);
            dialogue.ShowText(dialogueText[dialogueIndex]);
            isActive = true;
            dialogueIndex++;
            Debug.Log(dialogueIndex);
        } else if (isActive && dialogueAmount != dialogueIndex)
        {
            // update text
            dialogue.ShowText(dialogueText[dialogueIndex]);
            dialogueIndex++;
            Debug.Log(dialogueIndex);
        } else if (isActive && dialogueAmount == dialogueIndex)
        {
            cam.ResetCamera();
            if (velocityController != null)
                velocityController.canMove = true;
            dialogue.gameObject.SetActive(false);
            isActive = false;
        }

    }
}

