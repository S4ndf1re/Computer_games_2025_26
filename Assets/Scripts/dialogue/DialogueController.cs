using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [Header("References")]
    public Transform followTarget;                     // NPC oder Interactable
    public TextMeshProUGUI textField;                  // Das Text-Objekt im Prefab

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2f, 0);     // Position über NPC
    public float typewriterSpeed = 0.03f;              // Geschwindigkeit des Text-Effekts

    private Camera cam;
    private Coroutine typeRoutine;

    void Start()
    {
        cam = Camera.main;

        // Falls der Text im Editor nicht gesetzt wurde, automatisch suchen
        if (textField == null)
            textField = GetComponentInChildren<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        if (followTarget == null)
            return;

        // Position über Ziel setzen
        transform.position = followTarget.position + offset;

        // Billboard: zur Kamera drehen
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180f, 0);   // Canvas schaut sonst rückwärts
    }

    public void ShowText(string message)
    {
        // Text sofort löschen
        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        typeRoutine = StartCoroutine(TypeText(message));
    }

    public void Clear()
    {
        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        if (textField != null)
            textField.text = "";
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
}
