using UnityEngine;

public class PrinterStateCheck : MonoBehaviour
{

    public GameState state;
    public DialogueTrigger dialogueAfterTutorial;
    public DialogueTrigger dialogueWhenFinishing;
    public EnableAfterInteraction secretaryInteractable;
    public FinishTaskonInteraction printerTaskFinisher;
    public AddTaskOnInteraction findSecretaryTask;

    public SceneSwapTrigger toBossTrigger;

    void OnEnable()
    {

        var isPreBoss = state.hasInk && state.hasPaper;

        // before boss
        dialogueWhenFinishing.enabled = isPreBoss;
        toBossTrigger.enabled = isPreBoss;
        // after tutorial
        dialogueAfterTutorial.enabled = !isPreBoss;
        secretaryInteractable.enabled = !isPreBoss;
        printerTaskFinisher.enabled = !isPreBoss;
        findSecretaryTask.enabled = !isPreBoss;

    }
}
