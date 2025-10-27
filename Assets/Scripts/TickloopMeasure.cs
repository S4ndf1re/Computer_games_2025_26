using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TickloopMeasure : MonoBehaviour
{
    public TickloopBeat tickloopBeat;
    public List<TickloopBeat> beats = new List<TickloopBeat>();
    public Color active_color = Color.black;
    public Color inactive_color = Color.white;
    public Tickloop tickloop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstantiateBeats(int number_of_beats, Transform measure_transform)
    {
        for (int i = 0; i < number_of_beats; i++)
        {
            TickloopBeat beat = Instantiate(tickloopBeat, measure_transform);
            beat.active_color = this.active_color;
            beat.inactive_color = this.inactive_color;
            beat.tickloop = this.tickloop;
            beats.Add(Instantiate(tickloopBeat, measure_transform));
        }

    }

    public void HighlightBeat(int beat)
    {
        beats[beat].Highlight();
    }
    
    public void UnhighlightBeat(int beat)
    {
        beats[beat].Unhighlight();
    }
}
