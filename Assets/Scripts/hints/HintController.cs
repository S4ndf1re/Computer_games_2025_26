using System.Collections;
using UnityEngine;
using TMPro;

public class HintController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textField;

    [Header("Settings")]
    [SerializeField] private float typewriterSpeed = 0.03f;
    [SerializeField] private float lifetime = 10f;

    private Coroutine typeRoutine;
    private Coroutine lifeRoutine;

    private void Awake()
    {
        if (textField == null)
            textField = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Show(string message)
    {
        // Cleanup
        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        if (lifeRoutine != null)
            StopCoroutine(lifeRoutine);

        // Start routines
        typeRoutine = StartCoroutine(TypeText(message));
        lifeRoutine = StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator TypeText(string msg)
    {
        textField.text = "";

        foreach (char c in msg)
        {
            textField.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
