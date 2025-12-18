using UnityEngine;

public class SoundOnInteraction : MonoBehaviour, InteractableAction
{

    public AudioSource audio;

    public bool Execute()
    {
        audio.Play();
        return true;
    }
}
