using UnityEngine;

public class ActivateParticleSystemOnTrigger : MonoBehaviour
{

    public ParticleSystem system;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += OnTrigger;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= OnTrigger;
    }

    public void OnTrigger(Tickloop loop)
    {
        this.system.Play();

    }
}
