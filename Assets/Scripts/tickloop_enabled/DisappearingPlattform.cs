using UnityEngine;

public class DisappearingPlattform : MonoBehaviour
{
    [Header("Tickloop Source")]
    [Tooltip("If null, TickloopAddable on this GameObject will be used")]
    public TickloopAddable triggeredBy;

    MeshRenderer renderer;
    Collider collider;
    public bool invertEnabling = false;

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

        this.addable.triggeredByTickloop += Trigger;

        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        if (this.addable != null)
        {
            this.addable.triggeredByTickloop -= Trigger;
        }
    }

    void Trigger(Tickloop loop, int nth_trigger)
    {
        var enable = nth_trigger % 2 == 0;
        if (invertEnabling)
        {
            enable = !enable;
        }

        renderer.enabled = enable;
        collider.enabled = enable;
    }
}
