using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public delegate void OnHitTrigger(Hitbox box);
    public event OnHitTrigger onHitTriggerEvent;

    public bool mustExit = true;
    public float delayTillNextHitSeconds = 0.0f;

    private bool isIntersecting = false;
    private float intersectingSinceSeconds = 0.0f;
    private bool alreadyTriggered = false;


    void Start()
    {
        isIntersecting = false;
        intersectingSinceSeconds = 0.0f;
        alreadyTriggered = false;
    }

    void Update()
    {
        if (isIntersecting)
        {
            intersectingSinceSeconds += Time.deltaTime;
        }
    }

    public void ExitTrigger(Hitbox box)
    {
        isIntersecting = false;
        alreadyTriggered = false;
    }

    public void EnterTrigger(Hitbox box)
    {
        isIntersecting = true;
        intersectingSinceSeconds = 0.0f;
        // Set to false here, since the StayTrigger will  set this
        alreadyTriggered = false;
    }

    public void StayTrigger(Hitbox box)
    {
        if (mustExit && alreadyTriggered)
        {
            return;
        }

        if (mustExit) {
            onHitTriggerEvent?.Invoke(box);
            alreadyTriggered = true;
        } else {
            if (!alreadyTriggered) {
                onHitTriggerEvent?.Invoke(box);
                alreadyTriggered = true;
            } else {
                var delayTillNextHitLocal = delayTillNextHitSeconds;
                if (delayTillNextHitLocal < 0.00000001) {
                    // Trigger exactly once
                    delayTillNextHitLocal = Mathf.Max(0.0000001f, delayTillNextHitSeconds);
                }

                while (intersectingSinceSeconds >= delayTillNextHitLocal)
                {
                    onHitTriggerEvent?.Invoke(box);
                    intersectingSinceSeconds -= delayTillNextHitLocal;
                }
            }
        }

    }
}
