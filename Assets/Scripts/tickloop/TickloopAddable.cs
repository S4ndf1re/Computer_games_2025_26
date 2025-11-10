using System.Collections.Generic;
using UnityEngine;

public class TickloopAddable : MonoBehaviour
{

    public delegate void TriggeredByTickloop(Tickloop tickloop);
    public event TriggeredByTickloop triggeredByTickloop;

    public delegate void PhasedOutTick();
    public event PhasedOutTick phasedOutTickEvent;

    public List<int> ticksToTrigger = new List<int>();
    public int every_nth = 0;
    public int offset = 0;
    public int repeat = 0;
    public Sprite icon;
    public Color color = Color.white;
    public bool requestColor = false;

    public List<TickloopEnableCollider> enabledInColliders;

    // public GameObject tickloopObject;
    public Tickloop tickloop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // tickloop = tickloopObject.GetComponent<Tickloop>();


        if (every_nth > 0 && ticksToTrigger.Count == 0)
        {
            ticksToTrigger.Clear();
            for (int i = offset; i < tickloop.tickLength; i += every_nth)
            {
                for (int j = 0; j < repeat; j++)
                {
                    ticksToTrigger.Add(i + j);
                }
            }
        }

        if (enabledInColliders.Count == 0) {
            AddToTickloop();
        }


        foreach(var enabler in enabledInColliders) {
            enabler.enableEvent += AddToTickloop;
            enabler.disableEvent += RemoveFromTickloop;
        }
    }


    void OnDisable()
    {
        if(enabledInColliders.Count == 0) {
            RemoveFromTickloop();
        }

        foreach(var enabler in enabledInColliders) {
            enabler.enableEvent -= AddToTickloop;
            enabler.disableEvent -= RemoveFromTickloop;
        }
    }

    void RemoveFromTickloop() {
        this.tickloop.RemoveFromTickloop(gameObject);
    }

    void AddToTickloop() {
        if (!requestColor)
        {
            tickloop.AddToTickloop(gameObject, ticksToTrigger, Trigger, PhasedOut);
        }
        else
        {
            tickloop.AddToTickloop(gameObject, ticksToTrigger, Trigger, PhasedOut, ReceiveRandomColor);
        }
    }

    void Trigger()
    {
        triggeredByTickloop?.Invoke(this.tickloop);
    }

    void PhasedOut() {
        phasedOutTickEvent?.Invoke();
    }

    void ReceiveRandomColor(Color color)
    {
        this.color = color;
    }
}
