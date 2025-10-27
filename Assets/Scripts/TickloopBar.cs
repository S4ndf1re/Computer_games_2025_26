using UnityEngine;
using UnityEngine.UI;
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
        tickloop.AddUiTickDelegate(AnimateBar);
        // Alle Punkte inaktiv setzen

        float measureWidth = 0.0F;
        for (int i = 0; i < tickloop.numberOfMeasures; i++)
        {
            TickloopMeasure measure = Instantiate(tickloopMeasure, this.transform, false);

            HorizontalLayoutGroup measureHlg = measure.GetComponent<HorizontalLayoutGroup>(); 
            measureWidth = tickloop.beatsInMeasure * 20 + (tickloop.beatsInMeasure - 1) * measureHlg.spacing;

            RectTransform measureRect = measure.GetComponent<RectTransform>();
            measureRect.sizeDelta = new Vector2(measureWidth, 20);
            measure.InstantiateBeats(tickloop.beatsInMeasure, measure.transform);
            //measure.transform.SetParent(bar_transform, false);
            measures.Add(measure);
        }

        HorizontalLayoutGroup barHlg = GetComponent<HorizontalLayoutGroup>(); 
        RectTransform barRect = GetComponent<RectTransform>();
        barRect.sizeDelta = new Vector2(measureWidth * tickloop.numberOfMeasures + barHlg.spacing * (tickloop.numberOfMeasures - 1), 100);
    }

    void OnDisable()
    {
        tickloop.RemoveUiTickDelegate(AnimateBar);
    }

    void Update()
    {
        
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
