using UnityEngine;

public class Splash : MonoBehaviour
{
    public float decayRate = 0.1f;

    private SpriteRenderer renderer;


    void OnEnable()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var color = renderer.color;
        color.a -= decayRate * Time.deltaTime;
        renderer.color = color;

        if (color.a < decayRate)
        {
            Destroy(gameObject);
        }
    }


}
