using UnityEngine;

public class ActivateParticleSystemOnTrigger : MonoBehaviour
{

    [Header("Tickloop Source")]
    [Tooltip("If null, TickloopAddable on this GameObject will be used")]
    public TickloopAddable triggeredBy;

    public ParticleSystem system;

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

        this.addable.triggeredByTickloop += OnTrigger;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        if (this.addable != null)
        {
            this.addable.triggeredByTickloop -= OnTrigger;
        }
    }

    public void OnTrigger(Tickloop loop, int nth_trigger)
    {
        this.system.Play();
    }
}
