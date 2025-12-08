using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    private AudioSource source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();

        GetComponent<TickloopAddable>().triggeredByTickloop += PlaySound;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= PlaySound;
    }

    void PlaySound(Tickloop loop, int nth_trigger)
    {

        source.Play();

    }
}
