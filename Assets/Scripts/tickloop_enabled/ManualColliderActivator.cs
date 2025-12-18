using UnityEngine;

public class ManualColliderActivator : MonoBehaviour
{
    public float onTimeSeconds = 0.5f;
    public Collider toggleableCollider;
    public bool oneTimeUse = false;

    private bool used = false;
    private float lastEnabledSince = 0.0f;

    void OnTriggerEnter(Collider other)
    {
        if (!used)
        {
            EnableCollider();
        }

        if (oneTimeUse)
        {
            used = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    void EnableCollider()
    {
        lastEnabledSince = 0.0f;
        toggleableCollider.enabled = true;
    }
}
