using UnityEngine;
using System.Collections.Generic;

public class TickloopBar : MonoBehaviour
{
    public Transform bar_transform;
    public List<TickloopMeasure> measures = new List<TickloopMeasure>();
    public TickloopMeasure tickloop_measure;
    public float interval = 0.2f;
    public Tickloop tickloop;
    public int displayed_measures = 4;

    private float timer;

    void Start()
    {
        tickloop = GameObject.Find("Tickloop").GetComponent<Tickloop>();
        bar_transform = GameObject.Find("TickloopBar").GetComponent<Transform>();
        // Alle Punkte inaktiv setzen
        for(int i = 0; i < displayed_measures; i++)
        {
            TickloopMeasure measure = Instantiate(tickloop_measure, bar_transform, false);
            RectTransform rect_transform = measure.GetComponent<RectTransform>();
            rect_transform.sizeDelta = new Vector2(4*20, 20);
            measure.InstantiateBeats(tickloop.beatsInMeasure, measure.transform);
            //measure.transform.SetParent(bar_transform, false);
            measures.Add(measure);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            AnimateBar();
        }
    }

    private void AnimateBar()
    {
        // Alle inaktiv
        int current_measure = tickloop.currentIdx / tickloop.beatsInMeasure;
        int current_beat = tickloop.currentIdx % tickloop.beatsInMeasure;
        // Debug.Log(current_measure);
        // Debug.Log(current_beat);
        measures[current_measure].HighlightBeat(current_beat);

        int last_beat = current_beat - 1;
        if (last_beat == -1)
        {
            last_beat = 3;
        }
        int last_measure;
        if (last_beat == 3)
        {
            last_measure = current_measure - 1;
            if (last_measure == -1)
            {
                last_measure = 3;
            }
        }
        else
        {
            last_measure = current_measure;
        }
        measures[last_measure].UnhighlightBeat(last_beat);
    }
}
