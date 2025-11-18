using UnityEngine;

public class HigherGravity : MonoBehaviour
{
    public float gravity = 9.81f;
    private Rigidbody body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.useGravity = false;
        body.AddForce(Vector3.down * body.mass * gravity);
    }
}
