using UnityEngine;

public class ShootInk : MonoBehaviour
{

    public Ink inkPrefab;
    public LayerMask inkLayerFilter;


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
        Shoot();
    }


    public void Shoot()
    {
        var ink = Instantiate(inkPrefab);
        ink.Instantiate(transform.position, inkLayerFilter);
    }
}
