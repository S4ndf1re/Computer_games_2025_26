using System;
using System.Collections;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    public float duration = 0.5f;
    public float shakeMagnitude = 0.2f;
    public Camera mainCamera;
    public Hurtbox hurtbox;
    private bool isFrozen = false;
    private float pendingFreezeDuration = 0f;

    void Start()
    {
        if (hurtbox != null)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
        else
        {
            Debug.LogWarning("Warning: hurtbox is null");
        }
    }

    private void OnHit(Hitbox box)
    {
        Freeze();
    }

    // Update is called once per frame
    void Update()
    {
        if(pendingFreezeDuration > 0 && !isFrozen)
        {
            StartCoroutine(DoFreeze());
            StartCoroutine(ScreenShake());
        }
    }

    public void Freeze()
    {
        pendingFreezeDuration = duration;
    }

    IEnumerator DoFreeze()
    {
        isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        pendingFreezeDuration = 0f;
        isFrozen = false;
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

       mainCamera.transform.localPosition = originalPos;
    }

}
