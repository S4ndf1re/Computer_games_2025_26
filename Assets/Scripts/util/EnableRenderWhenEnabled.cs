using System.Collections;
using UnityEngine;

public class EnableRenderWhenEnabled : MonoBehaviour
{


    public TickloopAddable triggeredBy;
    public MeshRenderer renderer;
    public float durationSeconds = 0.1f;
    public int flashNTimes = 10;
    private Coroutine current;



    public void StartFlash()
    {
        if (current != null)
        {
            StopCoroutine(current);
            current = null;
        }
        current = StartCoroutine(Flash());
    }

    void Update()
    {
        if (current != null && !triggeredBy.enabled)
        {
            StopCoroutine(current);
            current = null;
        }
        renderer.enabled = triggeredBy.enabled;
    }

    IEnumerator Flash()
    {
        for (var i = 0; i < flashNTimes; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(durationSeconds);
        }
    }
}
