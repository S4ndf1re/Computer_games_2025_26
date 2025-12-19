using UnityEngine;

public class MetronomePlayer : MonoBehaviour
{
    [SerializeField] private Tickloop tickloop;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip downbeatClip;
    [SerializeField] private AudioClip beatClip;

    void OnEnable()
    {
        tickloop.audioTrigger += AudioTrigger;
    }

    void OnDisable()
    {
        tickloop.audioTrigger -= AudioTrigger;
    }

    public void AudioTrigger(bool isDownbeat)
    {
        if (isDownbeat)
            audioSource.PlayOneShot(downbeatClip);
        else
            audioSource.PlayOneShot(beatClip);
    }
}

