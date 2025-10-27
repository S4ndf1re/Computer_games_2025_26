using UnityEngine.UI;
using UnityEngine;

public class TickloopBeat : MonoBehaviour
{

    public Color active_color = Color.black;
    public Color inactive_color = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image image;
    public Tickloop tickloop;
    void Start()
    {
        this.image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Highlight()
    {
        this.image.color = active_color;
    }
    
    public void Unhighlight()
    {
        this.image.color = inactive_color;
    }
}
