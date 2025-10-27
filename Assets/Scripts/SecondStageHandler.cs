using UnityEngine;

public class SecondStageHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        TickloopAddable addable = GetComponent<TickloopAddable>();
        addable.triggeredByTickloop += Trigger;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        TickloopAddable addable = GetComponent<TickloopAddable>();
        addable.triggeredByTickloop -= Trigger;
    }

    void Trigger(Tickloop tickloop)
    {
        Debug.Log("Triggered within component itself");
    }
}
