using UnityEngine;

public class ActivateJumpAfterInteraction : MonoBehaviour, InteractableAction
{
    public VelocityPlayerController controller;


    public bool Execute()
    {
        controller.disableJump = false;
        return true;
    }
}
