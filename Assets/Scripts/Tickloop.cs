using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;
using Microsoft.Unity.VisualStudio.Editor;


public class Tickloop : MonoBehaviour
{
    public delegate void OnTriggeredTick();


    public delegate void OnAddedGameObject(GameObject obj, List<int> tickIndices);
    public delegate void OnRemoveGameObject(GameObject obj);

    public event OnAddedGameObject onAddedGameObject;
    public event OnRemoveGameObject onRemoveGameObject;
    
    public int tickLength = 0;
    public int currentIdx = 0;
    [SerializeField]
    private double currentTimeSeconds = 0.0;
    [SerializeField]
    private double secondsForBeats = 0.0;
    private List<OnTriggeredTick> uiTriggerTicks = new List<OnTriggeredTick>();

    [Delayed]
    public int bpm = 0;
    [Range(1, 6)]
    public int beatsInMeasure = 4;
    [Range(1, 6)]
    public int numberOfMeasures = 4;
    public bool repeat = true;
    public bool running = true;

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
        // NOTE: Start at end, since current tick must start at 0 after the first tick is triggered
        currentIdx = tickLength-1;
        currentTimeSeconds = 0.0;
        secondsForBeats = 1.0 / BpmToBps(bpm);

        ticks = new List<List<GameObject>>();
        for (int i = 0; i < tickLength; i++)
        {
            ticks.Add(new List<GameObject>());
        }
    }

    // Use FixedUpdate instead of Update, since it is better for tick stability, when the updates are always the same spacing
    void Update()
    {
        if (running)
        {
            this.currentTimeSeconds += Time.deltaTime;
        }


        var last_idx = (this.currentIdx - 1 + this.tickLength) % this.tickLength;

        // This is a little buggy, because bpm_as_seconds cant be compared like this
        while (this.currentTimeSeconds >= this.secondsForBeats)
        {

            if (running)
            {
                // Trigger Event, since we crossed the tick mark. 
                if (this.currentIdx >= 0 && this.currentIdx < this.ticks.Count)
                {
                    // Set tick to one more than actual tick. to represent tick change and start at 0
                    this.currentIdx = (this.currentIdx + 1) % this.tickLength;
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

                    // Undo change done previously
                    this.currentIdx = (this.currentIdx - 1 + this.tickLength) % this.tickLength;

                }

                this.currentIdx = (this.currentIdx + 1) % this.ticks.Count;
                if(!this.repeat)
                {
                    if (this.currentIdx >= this.tickLength-1)
                    {
                        this.running = false;
                    }
                }
            }

            this.currentTimeSeconds -= this.secondsForBeats;

        }

    }


    public void AddToTickloop(GameObject obj, List<int> ticksToTrigger, OnTriggeredTick delegateToRegister)
    {
        bool added = false;
        foreach (int idx in ticksToTrigger)
        {
            if (idx >= 0 && idx < this.ticks.Count)
            {
                this.ticks[idx].Add(obj);
                added = true;
            }
        }

        if (added)
        {
            this.objDelegateMapping.Add(obj, delegateToRegister);
            this.onAddedGameObject?.Invoke(obj, ticksToTrigger);
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
            this.objDelegateMapping.Remove(obj);
            this.onRemoveGameObject?.Invoke(obj);
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
