using UnityEngine;

public class RatBehaviour : MonoBehaviour
{
    public Player player;
    public float speed = 1.0f;
    public float distance = 1.0f;

    private Rigidbody body;

    // Movement related
    private bool isMoving = false;
    private float distanceMoved = 0.0f;
    private Vector3 direction = Vector3.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body != null && isMoving)
        {
            var diff = direction * speed * Time.deltaTime;
            body.transform.position += diff;
            distanceMoved += diff.magnitude;

            if (distanceMoved >= distance)
            {
                var distanceToTarget = distanceMoved - distance;
                var inverseDirection = -direction.normalized;

                // Reset so that the dash position is exact
                body.transform.position += inverseDirection * distanceToTarget;
                isMoving = false;
            }
        }
    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Trigger;
    }

    void Trigger(Tickloop loop)
    {
        if (player != null && body != null)
        {
            var targetPosition = player.transform.position;
            var startPosition = transform.position;
            var targetRotation = Quaternion.FromToRotation(startPosition, targetPosition);
            transform.rotation = targetRotation;

            var diff = (targetPosition - startPosition).normalized;
            isMoving = true;
            distanceMoved = 0.0f;
            direction = diff;
        }
    }
}
