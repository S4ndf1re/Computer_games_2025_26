using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    [Header("Settings")]
    public float focusDuration = 0.5f;
    public float zoomedFOV = 60f;
    public float rotationOffset = 0f;

    private Quaternion originalRot;
    private float originalFOV;

    private Coroutine routine;

    public bool isFocused { get; private set; } = false;
    public bool isResetting { get; private set; } = false;

    public Transform currentTarget = null;

    void Start()
    {
        cam = GetComponent<Camera>();

        originalFOV = cam.fieldOfView;
        originalRot = transform.rotation;
    }

    public void FocusOn(Transform target)
    {
        if (routine != null)
            StopCoroutine(routine);

        currentTarget = target;
        isResetting = false;
        isFocused = true;

        routine = StartCoroutine(FocusRoutine(target));
    }

    public void ResetCamera()
    {
        if (isResetting || !isFocused)
            return;

        if (routine != null)
            StopCoroutine(routine);

        isResetting = true;
        isFocused = false;
        currentTarget = null;

        routine = StartCoroutine(ResetRoutine());
    }

    private IEnumerator FocusRoutine(Transform target)
    {
        Quaternion startRot = transform.rotation;

        // Die Rotation, damit die Kamera auf das Objekt schaut
        Quaternion targetRot =
            Quaternion.LookRotation(target.position - transform.position);

        // Optional: etwas versetzen
        if (rotationOffset != 0f)
            targetRot *= Quaternion.Euler(0f, rotationOffset, 0f);

        float startFOV = cam.fieldOfView;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / focusDuration;
            float s = Mathf.SmoothStep(0, 1, t);

            transform.rotation = Quaternion.Slerp(startRot, targetRot, s);
            cam.fieldOfView = Mathf.Lerp(startFOV, zoomedFOV, s);

            yield return null;
        }
    }

    private IEnumerator ResetRoutine()
    {
        Quaternion startRot = transform.rotation;
        float startFOV = cam.fieldOfView;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / focusDuration;
            float s = Mathf.SmoothStep(0, 1, t);

            transform.rotation = Quaternion.Slerp(startRot, originalRot, s);
            cam.fieldOfView = Mathf.Lerp(startFOV, originalFOV, s);

            yield return null;
        }

        isResetting = false;
    }
}
