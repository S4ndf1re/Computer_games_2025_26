using UnityEngine;
using UnityEngine.Rendering;

public class Ink : MonoBehaviour
{

    public Splash splash;
    public float velocity = 20f;
    public Vector3 direction = Vector3.down;
    public float lifetimeSeconds = 2f;
    public LayerMask layerFilter;

    public void Instantiate(Vector3 startingPosition, LayerMask layerFilter)
    {
        transform.position = startingPosition;
        this.direction = Vector3.down;
        this.layerFilter = layerFilter;
        Destroy(gameObject, lifetimeSeconds);
    }
    public void Instantiate(Vector3 startingPosition, Vector3 direction, LayerMask layerFilter)
    {
        transform.position = startingPosition;
        this.direction = direction;
        this.layerFilter = layerFilter;
        Destroy(gameObject, lifetimeSeconds);
    }

    void FixedUpdate()
    {
        var old = transform.position;

        old += velocity * direction * Time.fixedDeltaTime;

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
