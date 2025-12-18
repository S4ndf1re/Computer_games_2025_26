using UnityEngine;
using System.Collections;

public class PlattformMoveAction : MonoBehaviour, InteractableAction
{
    [Header("Target to move")]
    public Transform targetToMove;

    [Header("Movement Settings")]
    public Vector3 moveOffset = new Vector3(0, 2f, 0);  // Wohin soll bewegt werden
    public float moveDuration = 1f;                     // Dauer der Bewegung
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine moveRoutine;

    public bool Execute()
    {
        if (targetToMove == null)
        {
            Debug.LogWarning($"PlattformMoveAction: Kein Target gesetzt bei {gameObject.name}.");
            return false;
        }

        // Bereits laufende Bewegung stoppen
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MovePlatform());
        return true;
    }

    private IEnumerator MovePlatform()
    {
        Vector3 startPos = targetToMove.position;
        Vector3 endPos = startPos + moveOffset;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            float eased = easeCurve.Evaluate(t);

            targetToMove.position = Vector3.Lerp(startPos, endPos, eased);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Exakt am Ziel landen
        targetToMove.position = endPos;

        moveRoutine = null;
    }


    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
