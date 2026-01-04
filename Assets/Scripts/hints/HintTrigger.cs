using UnityEngine;
using System.Collections.Generic;

public class HintTrigger : MonoBehaviour, InteractableAction
{
    [Header("Hint Content")]
    [TextArea]
    public List<string> hintTexts;

    private int hintIndex = 0;

    public bool Execute()
    {
        if (hintIndex >= hintTexts.Count)
            return true;

        HintManager.Instance.ShowHint(hintTexts[hintIndex]);
        hintIndex++;

        return false;
    }

    public void StartInteraction()
    {
        hintIndex = 0;
    }

    public void EndInteraction()
    {
        // nichts zu tun – Hints regeln ihr eigenes Lifetime
    }

    bool InteractableAction.IsActive()
    {
        return enabled;
    }
}
