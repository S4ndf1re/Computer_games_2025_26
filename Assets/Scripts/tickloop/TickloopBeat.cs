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


    private Dictionary<GameObject, Image> objToIconMapping = new Dictionary<GameObject, Image>();

    void Start()
    {
        this.image = GetComponent<Image>();
        objToIconMapping.Clear();
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

    public void AddObject(GameObject obj)
    {

        Debug.Log("Adding to beat");
        if (obj.TryGetComponent(out TickloopAddable addable) && addable.icon != null && addable.color != null)
        {
            Image newImage = Instantiate(iconPrefab, transform, false);
            newImage.sprite = addable.icon;
            newImage.color = addable.color;

            objToIconMapping.Add(obj, newImage);
        }

    }

    public void RemoveObject(GameObject obj)
    {

        if (objToIconMapping.ContainsKey(obj))
        {
            Image imgToRemove = objToIconMapping[obj];
            Destroy(imgToRemove.gameObject);
            objToIconMapping.Remove(obj);
        }

    }
}
