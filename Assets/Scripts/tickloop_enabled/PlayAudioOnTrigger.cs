using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    [Header("Tickloop Source")]
    [Tooltip("If null, TickloopAddable on this GameObject will be used")]
    public TickloopAddable triggeredBy;

    private AudioSource source;

    private TickloopAddable addable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (triggeredBy != null)
        {
            this.addable = this.triggeredBy;
        } else
        {
            this.addable = GetComponent<TickloopAddable>();
        }

        source = GetComponent<AudioSource>();

        this.addable.triggeredByTickloop += PlaySound;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        if (this.addable != null)
        {
            this.addable.triggeredByTickloop -= PlaySound;
        }
    }

    void PlaySound(Tickloop loop, int nth_trigger)
    {
        this.source.Play();
    }
}
