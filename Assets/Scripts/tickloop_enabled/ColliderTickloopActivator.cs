using UnityEngine;

public class ColliderTickloopActivator : MonoBehaviour
{
    [Header("Tickloop Source")]
    [Tooltip("If null, TickloopAddable on this GameObject will be used")]
    public TickloopAddable triggeredBy;

    public float onTimeSeconds = 0.5f;
    public Collider toggleableCollider;

    private float lastEnabledSince = 0.0f;

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

        this.addable.triggeredByTickloop += EnableCollider;
        toggleableCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleableCollider.enabled)
        {
            lastEnabledSince += Time.deltaTime;
        }

        if (lastEnabledSince >= onTimeSeconds)
        {
            toggleableCollider.enabled = false;
            lastEnabledSince = 0.0f;
        }

    }

    void OnDisable()
    {
        if (this.addable != null)
        {
            this.addable.triggeredByTickloop -= EnableCollider;
        }
    }


    void EnableCollider(Tickloop loop, int nth_trigger)
    {
        lastEnabledSince = 0.0f;
        toggleableCollider.enabled = true;
    }
}
