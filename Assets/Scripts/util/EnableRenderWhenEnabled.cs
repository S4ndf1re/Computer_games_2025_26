using System.Collections;
using UnityEngine;

public class EnableRenderWhenEnabled : MonoBehaviour
{


    public TickloopAddable triggeredBy;
    public MeshRenderer renderer;
    public float durationSeconds = 0.1f;
    public int flashNTimes = 10;
    private Coroutine current;
    private bool isFlashing;



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
        if (!isFlashing)
        {
            renderer.enabled = triggeredBy.enabled;
        }
    }

    IEnumerator Flash()
    {
        isFlashing = true;
        for (var i = 0; i < flashNTimes; i++)
        {
            renderer.enabled = !renderer.enabled;
            if (!triggeredBy.enabled)
            {
                renderer.enabled = false;
            }
            yield return new WaitForSeconds(durationSeconds);
        }
        isFlashing = false;
    }
}
