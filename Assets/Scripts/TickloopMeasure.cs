using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TickloopMeasure : MonoBehaviour
{
    public Image tickloopBeat;
    public List<Image> beats = new List<Image>();
    public Color active_color = Color.black;
    public Color inactive_color = Color.white;
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
            beats.Add(Instantiate(tickloopBeat, measure_transform));
        }

    }

    public void HighlightBeat(int beat)
    {
        beats[beat].color = active_color;
    }
    
    public void UnhighlightBeat(int beat)
    {
        beats[beat].color = inactive_color;
    }
}
