using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TickloopMeasure : MonoBehaviour
{
    public TickloopBeat tickloopBeat;
    public List<TickloopBeat> beats = new List<TickloopBeat>();
    public Color activeColor = Color.black;
    public Color inactiveColor = Color.white;
    public Tickloop tickloop;

    public void Instantiate(int number_of_beats, Transform measure_transform, Color activeColor, Color inactiveColor, Tickloop loop)
    {
        this.activeColor = activeColor;
        this.inactiveColor = inactiveColor;
        this.tickloop = loop;

        Vector2 beatDelta = Vector2.zero;
        for (int i = 0; i < number_of_beats; i++)
        {
            TickloopBeat beat = Instantiate(tickloopBeat, measure_transform);
            beat.Instantiate(this.activeColor, this.inactiveColor, this.tickloop);

            beatDelta = beat.getRectDelta();

            beats.Add(beat);
        }

        HorizontalLayoutGroup measureHlg = GetComponent<HorizontalLayoutGroup>();
        var measureWidth = tickloop.beatsInMeasure * beatDelta.x + (tickloop.beatsInMeasure - 1) * measureHlg.spacing;

        RectTransform measureRect = GetComponent<RectTransform>();
        measureRect.sizeDelta = new Vector2(measureWidth, beatDelta.y);
    }


    public Vector2 GetSizeDelta()
    {
        RectTransform measureRect = GetComponent<RectTransform>();

        return measureRect.sizeDelta;
    }


    public void HighlightBeat(int beat)
    {
        beats[beat].Highlight();
    }

    public void UnhighlightBeat(int beat)
    {
        beats[beat].Unhighlight();
    }

    public void UnhighlightAll()
    {
        foreach(var beat in beats)
        {
            beat.Unhighlight();
        }
    }

    public void AddObject(TickloopAddable obj, int idx)
    {
        this.beats[idx].AddObject(obj);
    }

    public void RemoveObject(TickloopAddable obj)
    {
        foreach (TickloopBeat beat in this.beats)
        {
            beat.RemoveObject(obj);
        }
    }
}
