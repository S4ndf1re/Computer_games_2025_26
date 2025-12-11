using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public delegate void OnHitTrigger(Hitbox box);
    public event OnHitTrigger onHitTriggerEvent;

    public bool mustExit = true;
    public bool ignoreHits = false;
    public float iFrameSeconds = 0.0f;

    [Header("Mesh Flash Settings")]
    public float flashTime = 0.0f;
    public List<Renderer> renderers;

    private bool alreadyTriggered = false;
    public bool isInIFrame = false;


    void Start()
    {
        alreadyTriggered = false;
    }

    public void ExitTrigger(Hitbox box)
    {
        alreadyTriggered = false;
    }

    public void EnterTrigger(Hitbox box)
    {
        // Set to false here, since the StayTrigger will  set this
        alreadyTriggered = false;
    }

    public void StayTrigger(Hitbox box)
    {
        if (mustExit && alreadyTriggered || ignoreHits || isInIFrame)
        {
            return;
        }

        StartCoroutine(Hit(box));
    }

    IEnumerator Hit(Hitbox box)
    {
        var flashRoutine = StartCoroutine(Flash());
        alreadyTriggered = true;
        this.onHitTriggerEvent?.Invoke(box);
        this.isInIFrame = true;
        yield return new WaitForSeconds(iFrameSeconds);
        this.isInIFrame = false;
        StopCoroutine(flashRoutine);
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }
    }


    IEnumerator Flash()
    {
        var setEnabled = false;
        while (true)
        {
            foreach (var renderer in renderers)
            {
                renderer.enabled = setEnabled;
            }
            setEnabled = !setEnabled;
            yield return new WaitForSeconds(flashTime);
        }
    }
}
