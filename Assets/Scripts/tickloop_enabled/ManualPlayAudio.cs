using UnityEngine;

public class ManualPlayAudio : MonoBehaviour
{
    public AudioSource source;
    public bool oneTimeUse = false;

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (!used)
        {
            this.source.Play();
        }

        if (oneTimeUse)
        {
            used = true;
        }
    }
}
