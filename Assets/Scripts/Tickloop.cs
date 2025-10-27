using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;


public class Tickloop : MonoBehaviour
{
    public delegate void OnTriggeredTick();
    
    public int tickLength = 0;
    public int currentIdx = 0;
    [SerializeField]
    private double currentTimeSeconds = 0.0;
    [SerializeField]
    private double secondsForBeats = 0.0;
    private List<OnTriggeredTick> uiTriggerTicks = new List<OnTriggeredTick>();

    [Delayed]
    public int bpm = 0;
    [Delayed]
    public int beatsInMeasure = 4;
    [Delayed]
    public int numberOfMeasures = 4;
    [Delayed]
    bool repeat = true;
    [Delayed]
    public bool finished = false;

    private List<List<GameObject>> ticks = new List<List<GameObject>>();
    private Dictionary<GameObject, OnTriggeredTick> objDelegateMapping = new Dictionary<GameObject, OnTriggeredTick>();



    private double BpmToBps(int bpm_in_minutes)
    {
        double bpm = (double)bpm_in_minutes;

        bpm /= 60.0;

        return bpm;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        tickLength = numberOfMeasures * beatsInMeasure;
        ticks.Clear();
        objDelegateMapping.Clear();
        uiTriggerTicks.Clear();
        currentIdx = 0;
        currentTimeSeconds = 0.0;
        secondsForBeats = 1.0 / BpmToBps(bpm);
        finished = false;

        ticks = new List<List<GameObject>>();
        for (int i = 0; i < tickLength; i++)
        {
            ticks.Add(new List<GameObject>());
        }
    }

    // Use FixedUpdate instead of Update, since it is better for tick stability, when the updates are always the same spacing
    void FixedUpdate()
    {
        this.currentTimeSeconds += Time.fixedDeltaTime;


        var last_idx = (this.currentIdx - 1 + this.tickLength) % this.tickLength;

        // This is a little buggy, because bpm_as_seconds cant be compared like this
        while (this.currentTimeSeconds >= this.secondsForBeats)
        {

            if (!finished)
            {
                // Trigger Event, since we crossed the tick mark. 
                if (this.currentIdx >= 0 && this.currentIdx < this.ticks.Count)
                {
                    foreach (var obj in this.ticks[this.currentIdx])
                    {
                        if (this.objDelegateMapping.ContainsKey(obj))
                        {
                            this.objDelegateMapping[obj].Invoke();
                        }
                    }

                    foreach (var func in this.uiTriggerTicks)
                    {
                        func.Invoke();
                    }

                }
                // Reset counters
                if (this.repeat)
                {
                    this.currentIdx = (this.currentIdx + 1) % this.ticks.Count;
                }
                else
                {
                    this.currentIdx = this.currentIdx + 1;
                    if (this.currentIdx >= this.ticks.Count)
                    {
                        this.finished = true;
                    }
                }
            }

            this.currentTimeSeconds -= this.secondsForBeats;

        }

    }


    public void AddToTickloop(GameObject obj, List<int> ticks_to_trigger, OnTriggeredTick delegate_to_register)
    {
        bool added = false;
        foreach (int idx in ticks_to_trigger)
        {
            if (idx >= 0 && idx < this.ticks.Count)
            {
                this.ticks[idx].Add(obj);
                added = true;
            }
        }

        if (added)
        {
            this.objDelegateMapping.Add(obj, delegate_to_register);
        }

    }

    public void RemoveFromTickloop(GameObject obj)
    {
        for (int i = 0; i < this.ticks.Count; i++)
        {
            this.ticks[i].Remove(obj);
        }

        if (this.objDelegateMapping.ContainsKey(obj))
        {
            var delegate_to_remove = this.objDelegateMapping[obj];
        }
    }

    public void AddUiTickDelegate(OnTriggeredTick delegate_function)
    {
        this.uiTriggerTicks.Add(delegate_function);
    }

    public void RemoveUiTickDelegate(OnTriggeredTick delegate_function)
    {
        this.uiTriggerTicks.Remove(delegate_function);
    }
}
