using UnityEngine;

public class DialogueTrigger : MonoBehaviour, InteractableAction
{
    public string dialogueText;
    public GameObject dialoguePrefab;

    private DialogueController dialogue;

    void Start()
    {
        var obj = Instantiate(dialoguePrefab);
        obj.SetActive(false);

        dialogue = obj.GetComponent<DialogueController>();
        dialogue.followTarget = transform;
    }

    public void Execute()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.ShowText(dialogueText);
    }
}

