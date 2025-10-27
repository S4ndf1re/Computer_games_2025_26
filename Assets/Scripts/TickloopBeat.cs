using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TickloopBeat : MonoBehaviour
{

    public Color active_color = Color.black;
    public Color inactive_color = Color.white;
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

    public void AddObject(GameObject obj)
    {

        if (obj.TryGetComponent(out TickloopAddable addable) && addable.icon != null)
        {
            Image newImage = Instantiate(iconPrefab, transform, false);
            newImage.sprite = addable.icon;

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
