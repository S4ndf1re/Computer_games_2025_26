using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TickloopBeat : MonoBehaviour
{

    public Color activeColor = Color.black;
    public Color inactiveColor = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image image;
    public Tickloop tickloop;

    public Image iconPrefab;


    private Dictionary<TickloopAddable, Image> objToIconMapping = new Dictionary<TickloopAddable, Image>();

    void Start()
    {
        this.image = GetComponent<Image>();
    }

    public void Instantiate(Color activeColor, Color inactiveColor, Tickloop loop)
    {
        this.activeColor = activeColor;
        this.inactiveColor = inactiveColor;
        this.tickloop = loop;

        var image = GetComponent<Image>();
        image.color = inactiveColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 getRectDelta()
    {
        RectTransform beatRect = GetComponent<RectTransform>();

        return beatRect.sizeDelta;
    }

    public void Highlight()
    {
        this.image.color = activeColor;
    }

    public void Unhighlight()
    {
        this.image.color = inactiveColor;
    }

    public void AddObject(TickloopAddable addable)
    {

        if (addable.icon != null && addable.color != null)
        {
            Image newImage = Instantiate(iconPrefab, transform, false);
            newImage.sprite = addable.icon;
            newImage.color = addable.color;

            objToIconMapping.Add(addable, newImage);
        }

    }

    public void RemoveObject(TickloopAddable addable)
    {
        if (objToIconMapping.ContainsKey(addable))
        {
            Image imgToRemove = objToIconMapping[addable];
            Destroy(imgToRemove.gameObject);
            objToIconMapping.Remove(addable);
        }

    }
}
