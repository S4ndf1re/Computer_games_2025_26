using UnityEngine;
using UnityEngine.Rendering;

public class Ink : MonoBehaviour
{

    public Splash splash;
    public float yVelocity = -20f;
    public float lifetimeSeconds = 2f;
    public LayerMask layerFilter;

    public void Instantiate(Vector3 startingPosition, LayerMask layerFilter)
    {
        transform.position = startingPosition;
        this.layerFilter = layerFilter;
        Destroy(gameObject, lifetimeSeconds);
    }

    void FixedUpdate()
    {
        var old = transform.position;

        old.y += yVelocity * Time.fixedDeltaTime;

        transform.position = old;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerFilter) != 0)
        {
            var closest = other.ClosestPoint(transform.position);
            var splashObj = GameObject.Instantiate(splash);
            var oldPos = closest;
            oldPos.y += 0.001f;
            splashObj.transform.position = oldPos;

        }
    }

}
