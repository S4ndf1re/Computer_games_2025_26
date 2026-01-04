using UnityEditor.Rendering.Universal;
using UnityEngine;

public class ShootOnTick : MonoBehaviour
{

    public TickloopAddable triggerBy;
    public Ink inkPrefab;
    public LayerMask inkCollisionMask;
    public EnableRenderWhenEnabled beamFlasher;
    private TickloopAddable addable;


    void OnEnable()
    {
        if (triggerBy != null)
        {
            addable = triggerBy;
        }
        else
        {
            addable = GetComponent<TickloopAddable>();
        }
        addable.triggeredByTickloop += Trigger;
    }

    void OnDisable()
    {
        addable.triggeredByTickloop -= Trigger;
    }


    void Trigger(Tickloop loop, int nthTrigger)
    {
        if (inkPrefab != null)
        {
            var ink = Instantiate(inkPrefab);
            ink.Instantiate(transform.position, transform.forward, inkCollisionMask);
            if (beamFlasher != null)
            {
                beamFlasher.StartFlash();
            }
        }

    }
}
