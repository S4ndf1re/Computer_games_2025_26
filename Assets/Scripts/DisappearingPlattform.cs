using UnityEngine;

public class DisappearingPlattform : MonoBehaviour
{
    MeshRenderer renderer;
    Collider collider;

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

    void OnDisable() {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Trigger;
    }

    void Trigger(Tickloop loop) {
        if (renderer.enabled) {
            renderer.enabled = false;
            collider.enabled = false;
        } else {
            renderer.enabled = true;
            collider.enabled = true;
        }
    }
}
