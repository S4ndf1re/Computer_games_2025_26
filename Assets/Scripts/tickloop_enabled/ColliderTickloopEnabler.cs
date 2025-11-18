using UnityEngine;

public class ColliderTickloopEnabler : MonoBehaviour
{
    public float onTimeSeconds = 0.5f;
    public Collider toggleableCollider;

    private float lastEnabledSince = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += EnableCollider;
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
        GetComponent<TickloopAddable>().triggeredByTickloop -= EnableCollider;
    }


    void EnableCollider(Tickloop loop)
    {
        lastEnabledSince = 0.0f;
        toggleableCollider.enabled = true;
    }
}
