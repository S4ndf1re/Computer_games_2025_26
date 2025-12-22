using System.Collections;
using UnityEngine;

public class PlayDelayed : MonoBehaviour
{

    public float delaySeconds = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlayAfterWait());
    }

    IEnumerator PlayAfterWait()
    {
        yield return new WaitForSeconds(delaySeconds);

        GetComponent<AudioSource>()?.Play();
    }

}
