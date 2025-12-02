using UnityEngine;
using System.Collections;

public class FadeOutOnTrigger : MonoBehaviour
{
    [Header("Target to fade")]
    public Renderer targetRenderer;

    [Header("Fade Settings")]
    public float fadeDuration = 1.0f;
    public bool disableRendererAtEnd = true;

    private Material targetMaterial;
    private Color originalColor;

    private void Start()
    {
        if (targetRenderer == null)
        {
            Debug.LogError("FadeOutOnTrigger: Kein Renderer gesetzt!", this);
            enabled = false;
            return;
        }
        targetMaterial = targetRenderer.material;
        originalColor = targetMaterial.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        targetRenderer.enabled = false;
        yield return null;
    }
}
