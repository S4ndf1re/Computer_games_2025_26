using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TickloopBar : MonoBehaviour
{
    public GameState state;
    public List<TickloopMeasure> measures = new List<TickloopMeasure>();
    public TickloopMeasure tickloopMeasure;
    public GameObject tickloopBeat;
    public float interval = 0.2f;
    public Tickloop tickloop;

    public Color activeColor = Color.black;
    public Color inactiveColor = Color.white;



    void OnEnable()
    {
        if (!state.tickloopEnabled)
        {
            return;
        }
        tickloop.uiTrigger += AnimateBar;
        tickloop.onAddedGameObject += AddObject;
        tickloop.onRemoveGameObject += RemoveObject;
        // Alle Punkte inaktiv setzen

        Vector2 measureDelta = Vector2.zero;
        for (int i = 0; i < tickloop.numberOfMeasures; i++)
        {
            TickloopMeasure measure = Instantiate(tickloopMeasure, this.transform, false);
            measure.Instantiate(tickloop.beatsInMeasure, measure.transform, this.activeColor, this.inactiveColor, this.tickloop);
            measureDelta = measure.GetSizeDelta();

            measures.Add(measure);
        }

        HorizontalLayoutGroup barHlg = GetComponent<HorizontalLayoutGroup>();
        RectTransform barRect = GetComponent<RectTransform>();
        barRect.sizeDelta = new Vector2(measureDelta.x * tickloop.numberOfMeasures + barHlg.spacing * (tickloop.numberOfMeasures - 1), 100);
    }

    void OnDisable()
    {
        tickloop.uiTrigger -= AnimateBar;
        tickloop.onAddedGameObject -= AddObject;
        tickloop.onRemoveGameObject -= RemoveObject;
    }


    (int, int) IndexToBarAndBeat(int idx)
    {
        return (idx / tickloop.beatsInMeasure, idx % tickloop.beatsInMeasure);
    }

    private void AnimateBar()
    {
        // int previousIndex = (tickloop.currentIdx - 1 + tickloop.tickLength) % tickloop.tickLength;

        var (currentMeasure, currentBeat) = IndexToBarAndBeat(tickloop.currentIdx);
        // var (previousMeasure, previousBeat) = IndexToBarAndBeat(previousIndex);

        foreach (var measure in measures)
        {
            measure.UnhighlightAll();
        }

        measures[currentMeasure].HighlightBeat(currentBeat);
        // measures[previousMeasure].UnhighlightBeat(previousBeat);
    }

    void AddObject(TickloopAddable obj, List<int> tickIndices)
    {
        foreach (int idx in tickIndices)
        {
            var (measure, beat) = IndexToBarAndBeat(idx);
            measures[measure].AddObject(obj, beat);
        }

    }

    void RemoveObject(TickloopAddable obj)
    {
        foreach (TickloopMeasure measure in measures)
        {
            measure.RemoveObject(obj);
        }

    }
}
