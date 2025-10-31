using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    private List<Color> colors;
    private int currentIdx;


    public ColorGenerator()
    {
        colors = new List<Color>();

        currentIdx = 0;

        colors.Add(newColor(238.0f, 99.0f, 82.0f));
        colors.Add(newColor(89.0f, 205.0f, 144.0f));
        colors.Add(newColor(63.0f, 167.0f, 214.0f));
        colors.Add(newColor(250.0f, 192.0f, 94.0f));
        colors.Add(newColor(247.0f, 157.0f, 132.0f));
    }


    public Color? NextColor()
    {
        if (currentIdx < colors.Count)
        {
            var color = colors[currentIdx];
            currentIdx = (currentIdx + 1) % colors.Count;
            return color;
        }
        return null;
    }

    Color newColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f);
    }

}
