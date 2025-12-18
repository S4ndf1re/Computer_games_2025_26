using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;


public class Tickloop : MonoBehaviour
{
    public delegate void OnTriggeredTick();
    public delegate void OnPhaseOutTick();
    public delegate void RequestRandomColor(Color color);


    public delegate void OnAddedGameObject(TickloopAddable obj, List<int> tickIndices);
    public delegate void OnRemoveGameObject(TickloopAddable obj);

    public event OnAddedGameObject onAddedGameObject;
    public event OnRemoveGameObject onRemoveGameObject;

    public event OnTriggeredTick uiTrigger;

    [Header("Debug Fields")]
    public int tickLength = 0;
    public int currentIdx = 0;
    [SerializeField]
    private double currentTimeSeconds = 0.0;
    [SerializeField]
    private double secondsForBeats = 0.0;

    [Header("Configuration")]
    [Delayed]
    public int bpm = 0;
    [Range(1, 6)]
    public int beatsInMeasure = 4;
    [Range(1, 6)]
    public int numberOfMeasures = 4;
    public bool repeat = true;
    public bool running = true;

    private List<List<TickloopAddable>> ticks = new List<List<TickloopAddable>>();
    private Dictionary<TickloopAddable, OnTriggeredTick> objDelegateMapping = new Dictionary<TickloopAddable, OnTriggeredTick>();
    private Dictionary<TickloopAddable, OnPhaseOutTick> objPhaseOutMapping = new Dictionary<TickloopAddable, OnPhaseOutTick>();
    private ColorGenerator colorGenerator = new ColorGenerator();


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
        objPhaseOutMapping.Clear();
        // NOTE: Start at end, since current tick must start at 0 after the first tick is triggered
        currentIdx = tickLength - 1;
        currentTimeSeconds = 0.0;
        secondsForBeats = 1.0 / BpmToBps(bpm);

        ticks = new List<List<TickloopAddable>>();
        for (int i = 0; i < tickLength; i++)
        {
            ticks.Add(new List<TickloopAddable>());
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

                    // Restart
                    if (this.currentIdx == this.ticks.Count - 1)
                    {
                        var cloned = objDelegateMapping.ToDictionary(entry => entry.Key,
                                                   entry => entry.Value);

                        foreach (var obj in cloned)
                        {
                            obj.Key.OnLoopStart();
                        }
                    }

                    // Phase out the object mapping
                    foreach (var obj in this.ticks[this.currentIdx])
                    {
                        if (this.objPhaseOutMapping.ContainsKey(obj))
                        {
                            this.objPhaseOutMapping[obj].Invoke();
                        }
                    }


                    // Set tick to one more than actual tick. to represent tick change and start at 0
                    this.currentIdx = (this.currentIdx + 1) % this.tickLength;
                    foreach (var obj in this.ticks[this.currentIdx])
                    {
                        if (this.objDelegateMapping.ContainsKey(obj))
                        {
                            this.objDelegateMapping[obj].Invoke();
                        }
                    }

                    uiTrigger?.Invoke();

                    // Undo change done previously
                    this.currentIdx = (this.currentIdx - 1 + this.tickLength) % this.tickLength;

                    // End loop
                    if (this.currentIdx == this.ticks.Count - 2)
                    {
                        var cloned = objDelegateMapping.ToDictionary(entry => entry.Key,
                                                   entry => entry.Value);
                        foreach (var obj in cloned)
                        {
                            obj.Key.OnLoopEnd();
                        }
                    }
                }

                this.currentIdx = (this.currentIdx + 1) % this.ticks.Count;
                if (!this.repeat)
                {
                    if (this.currentIdx >= this.tickLength - 1)
                    {
                        this.running = false;
                    }
                }
            }

            this.currentTimeSeconds -= this.secondsForBeats;

        }

    }


    /// <summary>
    /// Add a game object to the tick loop using the specified ticks.
    /// The game object may request a custom color using the optional RequestRandomColor delegate
    /// </summary>
    public void AddToTickloop(TickloopAddable obj, List<int> ticksToTrigger, OnTriggeredTick delegateToRegister, OnPhaseOutTick onPhaseOutTick, RequestRandomColor colorRequestor = null)
    {
        foreach (int idx in ticksToTrigger)
        {
            if (idx >= 0 && idx < this.ticks.Count)
            {
                this.ticks[idx].Add(obj);
            }
        }

        if (colorRequestor != null)
        {
            // supply random color choosen from a color pallete to the entity
            colorRequestor.Invoke(RequestColor());
        }
        this.objDelegateMapping.Add(obj, delegateToRegister);
        this.objPhaseOutMapping.Add(obj, onPhaseOutTick);

        this.onAddedGameObject?.Invoke(obj, ticksToTrigger);

    }

    /// <summary>
    /// Remove an TickloopAddable from the tickloop, deregistering it and removing it from the ui
    /// </summary>
    public void RemoveFromTickloop(TickloopAddable obj)
    {
        for (int i = 0; i < this.ticks.Count; i++)
        {
            this.ticks[i].Remove(obj);
        }

        if (this.objDelegateMapping.ContainsKey(obj))
        {
            this.objDelegateMapping.Remove(obj);
            this.objPhaseOutMapping.Remove(obj);
            this.onRemoveGameObject?.Invoke(obj);
        }
    }

    /// <summary>
    /// Request a pseudo Random color from an internal color generator
    /// </summary>
    public Color RequestColor()
    {
        var color = colorGenerator.NextColor();

        return color ?? Color.white;
    }
}
