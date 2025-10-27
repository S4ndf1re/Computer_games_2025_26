using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework.Constraints;
using UnityEngine;

public class TickloopAddable : MonoBehaviour
{

    public delegate void TriggeredByTickloop(Tickloop tickloop);
    public event TriggeredByTickloop triggeredByTickloop;

    public List<int> ticksToTrigger = new List<int>();
    public int every_nth = 0;
    public int offset = 0;
    public Sprite icon;

    public GameObject tickloopObject;
    private Tickloop tickloop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tickloop = tickloopObject.GetComponent<Tickloop>();


        if (every_nth > 0 && ticksToTrigger.Count == 0)
        {
            ticksToTrigger.Clear();
            for (int i = offset; i < tickloop.tickLength; i += every_nth)
            {
                ticksToTrigger.Add(i);
            }
        }

        tickloop.AddToTickloop(gameObject, ticksToTrigger, Trigger);
    }


    void OnDisable()
    {
        this.tickloop.RemoveFromTickloop(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Trigger()
    {
        triggeredByTickloop?.Invoke(this.tickloop);
        Debug.Log(tickloop.currentIdx);
    }
}
