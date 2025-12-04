using UnityEngine;
using TMPro;
using System.Collections;

public class Task : MonoBehaviour
{
    public AudioSource audio;
    public Animator animator;
    public float wait = 1;
    public TextMeshProUGUI textField;
    public float typewriterSpeed;
    public TaskCondition condition;

    public IEnumerator TypeText(string msg)
    {
        textField.text = "";

        foreach (char c in msg)
        {
            textField.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }

    public void ShowTextInstant(string msg)
    {
        textField.text = msg;
    }

    public void TriggerDespawn()
    {
        animator.SetTrigger("start");
        audio.Play();
        StartCoroutine(WaitAndDespawn());
    }

    private IEnumerator WaitAndDespawn()
    {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
