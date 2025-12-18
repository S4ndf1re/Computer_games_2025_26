using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoveOnTick : MonoBehaviour
{

    public Vector3 velocity;
    public float moveForSeconds = 0.0f;
    public Vector3 startPosition;
    private bool isMoving;
    private Coroutine currentlyRunning;

    void OnEnable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
        GetComponent<TickloopAddable>().onEnable += OnEnableAddable;
        GetComponent<TickloopAddable>().onDisable += OnDisableAddable;
    }
    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Trigger;
        GetComponent<TickloopAddable>().onEnable -= OnEnableAddable;
        GetComponent<TickloopAddable>().onDisable -= OnDisableAddable;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            var old = transform.position;
            old += velocity * Time.fixedDeltaTime;
            transform.position = old;
        }
    }

    void Trigger(Tickloop loop, int nth_trigger)
    {
        if (currentlyRunning != null)
        {
            StopCoroutine(currentlyRunning);
        }
        currentlyRunning = StartCoroutine(StartAndStopMove());
    }

    IEnumerator StartAndStopMove()
    {
        isMoving = true;
        yield return new WaitForSeconds(moveForSeconds);
        isMoving = false;
    }

    void OnEnableAddable()
    {
        this.transform.position = startPosition;
    }

    void OnDisableAddable()
    {
        isMoving = false;
        if (currentlyRunning != null)
        {
            StopCoroutine(currentlyRunning);
        }
        currentlyRunning = StartCoroutine(FadeDown());
    }


    IEnumerator FadeDown()
    {
        var pos = transform.position;
        var startY = transform.position.y;
        Debug.Log("Start Y: " + startY);
        while (pos.y > -startY)
        {
            pos.y -= 0.01f;
            transform.position = pos;
            Debug.Log("Current Y: " + pos.y);
            yield return new WaitForSeconds(0.1f);
        }
        this.transform.position = startPosition;
    }
}
