using UnityEngine;

public class DespawnAfterSeconds : MonoBehaviour
{
    public float durationSeconds = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, durationSeconds);
    }
}
