using System.Collections;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    public float duration = 0.1f;
    public float shakeMagnitude = 0.2f;
    public Camera mainCamera;
    public Hurtbox hurtbox;

    private bool isFreezing = false;

    void Start()
    {
        if (hurtbox != null)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
    }

    private void OnHit(Hitbox box)
    {
        if (!isFreezing)
            StartCoroutine(FreezeRoutine());
    }

    private IEnumerator FreezeRoutine()
    {
        isFreezing = true;

        // --- FREEZE ---
        Time.timeScale = 0f;

        // Shake parallel
        StartCoroutine(ScreenShake());

        // Unscaled warten
        yield return new WaitForSecondsRealtime(duration);

        // --- UNFREEZE (hart) ---
        Time.timeScale = 1f;
        isFreezing = false;
    }

    private IEnumerator ScreenShake()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    // Failsafe
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
