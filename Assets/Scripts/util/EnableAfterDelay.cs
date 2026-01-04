using System.Collections;
using UnityEngine;

public class EnableAfterDelay : MonoBehaviour
{


    public float delay = 0f;
    public MonoBehaviour behavior;


    void Start()
    {
        StartCoroutine(EnableDelayed());
    }

    IEnumerator EnableDelayed()
    {
        yield return new WaitForSeconds(delay);
        behavior.enabled = true;
    }
}
