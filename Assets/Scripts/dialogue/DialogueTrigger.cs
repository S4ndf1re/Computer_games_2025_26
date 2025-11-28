using UnityEngine;

public class DialogueTrigger : MonoBehaviour, InteractableAction
{
    public string dialogueText;
    public GameObject dialoguePrefab;
    public VelocityPlayerController velocityController;

    private DialogueController dialogue;
    private CameraController cam;
    private bool isActive;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
        var obj = Instantiate(dialoguePrefab);
        obj.SetActive(false);

        dialogue = obj.GetComponent<DialogueController>();
        dialogue.followTarget = transform;
    }

    public void Execute()
    {
        if (!isActive)
        {
            cam.FocusOn(transform);
            if (velocityController != null)
                velocityController.canMove = false;
            dialogue.gameObject.SetActive(true);
            dialogue.ShowText(dialogueText);
            isActive = true;
        } else
        {
            cam.ResetCamera();
            if (velocityController != null)
                velocityController.canMove = true;
            isActive = false;
        }
    }
}

