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


    private Dictionary<int, Image> objToIconMapping = new Dictionary<int, Image>();

    void Start()
    {
        this.image = GetComponent<Image>();
        objToIconMapping.Clear();
    }

    public void Instantiate(Color activeColor, Color inactiveColor, Tickloop loop)
    {
        Debug.Log("Instantiating Beat");
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

        Debug.Log("Trying to add " + addable.name + " in " + GetInstanceID());
        if (addable.icon != null && addable.color != null)
        {
            Image newImage = Instantiate(iconPrefab, transform, false);
            newImage.sprite = addable.icon;
            newImage.color = addable.color;

            objToIconMapping.Add(addable.GetInstanceID(), newImage);
            Debug.Log("Added " + addable.name + " " + addable.GetInstanceID() + " contains key: " + objToIconMapping.ContainsKey(addable.GetInstanceID()));
        }

    }

    public void RemoveObject(TickloopAddable addable)
    {
        Debug.Log("Bar contains Key: " + objToIconMapping.ContainsKey(addable.GetInstanceID()) + " for " + addable.name + " " + addable.GetInstanceID() + " in " + GetInstanceID());

        foreach (var (k, _) in objToIconMapping)
        {
            Debug.Log(k);
            Debug.Log(k == addable.GetInstanceID());
        }

        if (objToIconMapping.ContainsKey(addable.GetInstanceID()))
        {
            Image imgToRemove = objToIconMapping[addable.GetInstanceID()];
            Destroy(imgToRemove.gameObject);
            objToIconMapping.Remove(addable.GetInstanceID());
        }

    }
}
