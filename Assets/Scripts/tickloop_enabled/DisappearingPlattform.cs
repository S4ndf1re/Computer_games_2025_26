using UnityEngine;

public class DisappearingPlattform : MonoBehaviour
{
    MeshRenderer renderer;
    Collider collider;
    public bool invertEnabling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Trigger;
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
