using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TickloopAddable allows a GameObject to be added to a tickloop instance. Simply add it to a GameObject as a component.
/// It is required to contain an Image as a reference.
/// Optionally, The color can be requested automatically using a random color provided by the Tickloop itself.
///
/// Using the fields every_nth, offset and repeat, a beat may be generated.
/// Alternatively, manual ticks can be choosen by adding indices to the ticksToTrigger list. If at least one manual Tick is provided, the tick generation is disabled.
/// </summary>
public class TickloopAddable : MonoBehaviour
{

    public delegate void TriggeredByTickloop(Tickloop tickloop);
    public event TriggeredByTickloop triggeredByTickloop;

    public delegate void PhasedOutTick();
    public event PhasedOutTick phasedOutTickEvent;

    [Header("Manual Tick Settings")]
    public List<int> ticksToTrigger = new List<int>();

    [Header("Automatic Tick Generation")]
    [Tooltip("Repeat every n ticks")]
    public int every_nth = 4;
    [Tooltip("After the last tick (without repeat), apply an offset for the next beat. Starts at 0")]
    public int offset = 0;
    [Tooltip("After a nth tick, repeat the tick r times without gaps")]
    public int repeat = 1;

    [Header("Display Properties")]
    public Sprite icon;
    public Color color = Color.white;
    [Tooltip("When true, request a random color from the tickloop")]
    public bool requestColor = false;

    [Header("Collider Selection")]
    public List<TickloopEnableCollider> enabledInColliders;

    [Header("Tickloop Selection")]
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


        var addedEnabler = false;


        foreach (var enabler in enabledInColliders)
        {
            if (enabler != null)
            {
                enabler.enableEvent += AddToTickloop;
                enabler.disableEvent += RemoveFromTickloop;
                addedEnabler = true;
            }
        }

        if (!addedEnabler)
        {
            AddToTickloop();
        }
    }


    void OnDisable()
    {
        if (enabledInColliders.Count == 0)
        {
            RemoveFromTickloop();
        }

        foreach (var enabler in enabledInColliders)
        {
            enabler.enableEvent -= AddToTickloop;
            enabler.disableEvent -= RemoveFromTickloop;
        }
    }

    void RemoveFromTickloop()
    {
        this.tickloop.RemoveFromTickloop(this);
    }

    void AddToTickloop()
    {
        if (!requestColor)
        {
            tickloop.AddToTickloop(this, ticksToTrigger, Trigger, PhasedOut);
        }
        else
        {
            tickloop.AddToTickloop(this, ticksToTrigger, Trigger, PhasedOut, ReceiveRandomColor);
        }
    }

    void Trigger()
    {
        triggeredByTickloop?.Invoke(this.tickloop);
    }

    void PhasedOut()
    {
        phasedOutTickEvent?.Invoke();
    }

    void ReceiveRandomColor(Color color)
    {
        this.color = color;
    }
}
