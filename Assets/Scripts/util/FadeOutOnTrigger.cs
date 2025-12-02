using UnityEngine;
using System.Collections.Generic;

public class InstantHideOnTrigger : MonoBehaviour
{
    [Header("Parent whose children will be hidden")]
    public Transform targetParent;

    private List<Renderer> renderers = new List<Renderer>();

    private void Start()
    {
        if (targetParent == null)
        {
            Debug.LogError("InstantHideOnTrigger: Kein Parent gesetzt!", this);
            enabled = false;
            return;
        }

        // alle Renderer im gesamten Hierarchiebaum sammeln
        renderers.AddRange(targetParent.GetComponentsInChildren<Renderer>());
    }

    private void OnTriggerEnter(Collider other)
    {
        // sofort unsichtbar machen
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }
}
