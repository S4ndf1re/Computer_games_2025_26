using System;
using System.Collections;
using UnityEngine;

public class PrintHead : MonoBehaviour
{

    [Header("Slider parts")]
    public Transform xSlider;
    public Transform zSlider;
    public Transform printHead;

    public Vector2 currentPosition;
    public Vector2 homePosition;
    public bool isHoming;

    [Header("Velocity")]
    public float maxVelocity = 0f;
    public float timeToMoveSeconds = 1f;

    [Header("Bounds")]
    public Vector2 boundsMax;
    public Vector2 boundsMin;


    [Header("PID")]
    public bool followPlayer = false;
    public float p;
    public float i;
    public float d;
    public Transform player;
    public Vector2 lastError = Vector2.zero;
    public Vector2 sumError = Vector2.zero;

    private Coroutine currentlyRunning;


    void OnEnable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
        GetComponent<TickloopAddable>().onDisable += OnDisableAddable;
    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Trigger;
        GetComponent<TickloopAddable>().onDisable -= OnDisableAddable;
    }

    // Update is called once per frame
    void Update()
    {
        {
            var oldPos = xSlider.transform.position;
            oldPos.x = currentPosition.x;
            xSlider.transform.position = oldPos;
        }

        {
            var oldPos = zSlider.transform.position;
            oldPos.z = currentPosition.y; // y, since a Vec2
            zSlider.transform.position = oldPos;
        }

        {
            var oldPos = printHead.transform.position;
            oldPos.x = currentPosition.x;
            oldPos.z = currentPosition.y; // y since a Vec2
            printHead.transform.position = oldPos;
        }
    }

    float PID(float err, float sumError, float lastError, float timeDelta)
    {

        var pidValue = 0.0f;
        if (Math.Abs(i) > 0.0)
        {
            pidValue += p * timeDelta / i * sumError;
        }

        pidValue += p * err + p * d / timeDelta * (err - lastError) / timeDelta;
        return pidValue;
    }

    void FixedUpdate()
    {
        if (followPlayer)
        {
            var xDist = player.transform.position.x - currentPosition.x;
            var zDist = player.transform.position.z - currentPosition.y; // y, since its a Vec2
            var error = new Vector2(xDist, zDist);

            sumError += error;

            var newX = PID(xDist, sumError.x, lastError.x, Time.fixedDeltaTime);
            var newZ = PID(zDist, sumError.y, lastError.y, Time.fixedDeltaTime);

            var deltaV = (new Vector2(newX, newZ) - currentPosition) / Time.fixedDeltaTime;
            deltaV = Vector2.ClampMagnitude(deltaV, maxVelocity);

            currentPosition.x += deltaV.x * Time.fixedDeltaTime;
            currentPosition.y += deltaV.y * Time.fixedDeltaTime;

            lastError = error;
        }
        else if (isHoming)
        {
            var xDist = homePosition.x - currentPosition.x;
            var zDist = homePosition.y - currentPosition.y; // y, since its a Vec2
            var error = new Vector2(xDist, zDist);

            sumError += error;

            var newX = PID(xDist, sumError.x, lastError.x, Time.fixedDeltaTime);
            var newZ = PID(zDist, sumError.y, lastError.y, Time.fixedDeltaTime);

            var deltaV = (new Vector2(newX, newZ) - currentPosition) / Time.fixedDeltaTime;
            deltaV = Vector2.ClampMagnitude(deltaV, maxVelocity);

            currentPosition.x += deltaV.x * Time.fixedDeltaTime;
            currentPosition.y += deltaV.y * Time.fixedDeltaTime;

            lastError = error;
        }
    }


    IEnumerator AllowMovement()
    {
        this.followPlayer = true;
        this.isHoming = false;
        yield return new WaitForSeconds(this.timeToMoveSeconds);
        this.followPlayer = false;
    }

    void Trigger(Tickloop loop, int nth_trigger)
    {
        if (currentlyRunning != null)
        {
            StopCoroutine(currentlyRunning);
        }
        currentlyRunning = StartCoroutine(AllowMovement());
    }

    void OnDisableAddable()
    {
        if (currentlyRunning != null)
        {
            StopCoroutine(currentlyRunning);
        }
        this.isHoming = true;
        this.followPlayer = false;
    }
}
