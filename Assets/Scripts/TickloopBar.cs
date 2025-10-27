using UnityEngine;
using System.Collections.Generic;

public class TickloopBar : MonoBehaviour
{
    public List<TickloopMeasure> measures = new List<TickloopMeasure>();
    public TickloopMeasure tickloopMeasure;
    public float interval = 0.2f;
    private Tickloop tickloop;
    public GameObject tickloopObject;

    private float timer;

    void Start()
    {
        tickloop = tickloopObject.GetComponent<Tickloop>();
        // Alle Punkte inaktiv setzen
        for(int i = 0; i < tickloop.numberOfMeasures; i++)
        {
            TickloopMeasure measure = Instantiate(tickloopMeasure, this.transform, false);
            RectTransform rectTransform = measure.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(4*20, 20);
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

        int currentMeasure = tickloop.currentIdx / tickloop.beatsInMeasure;
        int currentBeat = tickloop.currentIdx % tickloop.beatsInMeasure;

        int previousIndex = (tickloop.currentIdx - 1 + tickloop.tickLength) % tickloop.tickLength;
        int previousMeasure = previousIndex / tickloop.beatsInMeasure;
        int previousBeat = previousIndex % tickloop.beatsInMeasure;

        measures[currentMeasure].HighlightBeat(currentBeat);
        measures[previousMeasure].UnhighlightBeat(previousBeat);
    }
}
