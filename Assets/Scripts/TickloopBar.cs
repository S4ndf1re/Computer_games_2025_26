using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TickloopBar : MonoBehaviour
{
    public List<TickloopMeasure> measures = new List<TickloopMeasure>();
    public TickloopMeasure tickloopMeasure;
    public GameObject tickloopBeat;
    public float interval = 0.2f;
    private Tickloop tickloop;
    public GameObject tickloopObject;

    public Color activeColor = Color.black;
    public Color inactiveColor = Color.white;

    void Start()
    {
        tickloop = tickloopObject.GetComponent<Tickloop>();
        tickloop.uiTrigger += AnimateBar;
        // Alle Punkte inaktiv setzen

        float measureWidth = 0.0F;
        RectTransform beatRect = tickloopBeat.GetComponent<RectTransform>();
        for (int i = 0; i < tickloop.numberOfMeasures; i++)
        {
            TickloopMeasure measure = Instantiate(tickloopMeasure, this.transform, false);
            measure.active_color = this.activeColor;
            measure.inactive_color = this.inactiveColor;
            HorizontalLayoutGroup measureHlg = measure.GetComponent<HorizontalLayoutGroup>(); 
            measureWidth = tickloop.beatsInMeasure * beatRect.sizeDelta.x + (tickloop.beatsInMeasure - 1) * measureHlg.spacing;

            RectTransform measureRect = measure.GetComponent<RectTransform>();
            measureRect.sizeDelta = new Vector2(measureWidth, beatRect.sizeDelta.y);
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
        tickloop.uiTrigger -= AnimateBar;
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
