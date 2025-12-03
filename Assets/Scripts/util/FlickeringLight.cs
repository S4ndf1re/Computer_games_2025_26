using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light myLight;

    public float maxDuration = 1;
    public float interval = 1;
    float timer;


    void Start() {
        if (myLight == null) {
            myLight = GetComponent<Light>();
        }
    }

    // Taken from https://gamedevbeginner.com/how-to-make-a-light-flicker-in-unity/
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            myLight.enabled = !myLight.enabled;
            interval = Random.Range(0f, maxDuration);
            timer = 0;
        }
    }
}
