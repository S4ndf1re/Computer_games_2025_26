using UnityEngine;

using System.Collections;
using System.Collections.Generic;


public class Tickloop : MonoBehaviour
{
    public delegate void OnTriggeredTick();

    public int bpm = 0;
    public int tick_length = 0;
    public bool repeat = true;
    public bool finished = false;
    public int current_idx = 0;
    public double current_time_seconds = 0.0;
    public double bpm_as_seconds = 0.0;

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
    void Start()
    {
        gameObjects.Clear();
        ticks.Clear();
        obj_delegate_mapping.Clear();
        current_idx = 0;
        current_time_seconds = 0.0;
        bpm_as_seconds = BpmToSeconds(bpm);
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

        // This is a little buggy, because bpm_as_seconds cant be compared like this
        while (this.current_time_seconds >= this.bpm_as_seconds)
        {

            // Trigger Event, since we crossed the tick mark. 
            if (!this.finished && this.current_idx >= 0 && this.current_idx < this.ticks.Count)
            {
                foreach (var obj in this.ticks[this.current_idx])
                {
                    if (this.obj_delegate_mapping.ContainsKey(obj))
                    {
                        this.obj_delegate_mapping[obj].Invoke();
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

            this.current_time_seconds -= this.bpm_as_seconds;

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
