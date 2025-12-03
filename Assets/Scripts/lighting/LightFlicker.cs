using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light lamp;

    [Header("Intensity Settings")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;

    [Header("Timing Settings")]
    public float minSpeed = 0.05f;   // klein = schnelles Flackern
    public float maxSpeed = 0.2f;    // groß = langsames Flackern

    private float targetIntensity;
    private float changeSpeed;

    void Start()
    {
        lamp = GetComponent<Light>();
        PickNewValues();
    }

    void Update()
    {
        // Smooth random flicker
        lamp.intensity = Mathf.Lerp(lamp.intensity, targetIntensity, Time.deltaTime * (1f / changeSpeed));

        // Wenn nah genug erreicht → neue Zufallswerte
        if (Mathf.Abs(lamp.intensity - targetIntensity) < 0.05f)
        {
            PickNewValues();
        }
    }

    void PickNewValues()
    {
        targetIntensity = Random.Range(minIntensity, maxIntensity);
        changeSpeed = Random.Range(minSpeed, maxSpeed);
    }
}
