using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;


public class Tickloop : MonoBehaviour
{
    public delegate void OnTriggeredTick();

    [Delayed]
    public int bpm = 0;

    [Delayed]
    public int tick_length = 0;
    public bool repeat = true;
    public bool finished = false;
    public int current_idx = 0;
    public double current_time_seconds = 0.0;
    public double second_for_beat = 0.0;
    public const int beats_in_measure = 4;

    private List<GameObject> gameObjects = new List<GameObject>();
    private List<List<GameObject>> ticks = new List<List<GameObject>>();
    private Dictionary<GameObject, OnTriggeredTick> obj_delegate_mapping = new Dictionary<GameObject, OnTriggeredTick>();



    private double BpmToSeconds(int bpm_in_minutes)
    {
        double bpm = (double)bpm_in_minutes;

        bpm /= 60.0;

        return bpm;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        gameObjects.Clear();
        ticks.Clear();
        obj_delegate_mapping.Clear();
        current_idx = 0;
        current_time_seconds = 0.0;
        second_for_beat = 1.0 / BpmToSeconds(bpm);
        finished = false;

        ticks = new List<List<GameObject>>();
        for (int i = 0; i < tick_length; i++)
        {
            ticks.Add(new List<GameObject>());
        }
    }

    // Use FixedUpdate instead of Update, since it is better for tick stability, when the updates are always the same spacing
    void FixedUpdate()
    {
        this.current_time_seconds += Time.fixedDeltaTime;


        var last_idx = (this.current_idx - 1 + this.tick_length) % this.tick_length;

        // This is a little buggy, because bpm_as_seconds cant be compared like this
        while (this.current_time_seconds >= this.second_for_beat)
        {

            if (!finished)
            {
                // Trigger Event, since we crossed the tick mark. 
                if (this.current_idx >= 0 && this.current_idx < this.ticks.Count)
                {
                    foreach (var obj in this.ticks[this.current_idx])
                    {
                        if (this.obj_delegate_mapping.ContainsKey(obj))
                        {
                            this.obj_delegate_mapping[obj].Invoke();
                        }
                    }

                }
                // Reset counters
                if (this.repeat)
                {
                    this.current_idx = (this.current_idx + 1) % this.ticks.Count;
                }
                else
                {
                    this.current_idx = this.current_idx + 1;
                    if (this.current_idx >= this.ticks.Count)
                    {
                        this.finished = true;
                    }
                }
            }

            this.current_time_seconds -= this.second_for_beat;

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
            this.obj_delegate_mapping.Add(obj, delegate_to_register);
        }

    }

    public void RemoveFromTickloop(GameObject obj)
    {
        for (int i = 0; i < this.ticks.Count; i++)
        {
            this.ticks[i].Remove(obj);
        }

        if (this.obj_delegate_mapping.ContainsKey(obj))
        {
            var delegate_to_remove = this.obj_delegate_mapping[obj];
        }
    }
}
