using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateRoundYOnTick : MonoBehaviour
{


    public float step = 0.01f;

    void OnEnable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Trigger;
    }

    void OnDisable()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Trigger;
    }


    void Trigger(Tickloop loop, int nth_trigger)
    {

    }


    IEnumerator Rotate()
    {

        var currentAngle = 0;
        var startAngle = transform.rotation.y;

        // TODO
        yield return new WaitForSeconds(0.1f);
    }
}
