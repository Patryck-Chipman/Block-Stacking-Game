using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColor : MonoBehaviour
{
    /// <summary>
    /// Field <c>colors</c> is a list of the available colors each object can choose from
    /// </summary>
    public Color[] colors { get; private set; }

    private const int COLOR_COUNT = 5;

    private void Start()
    {
        AddColors();
    }

    // Create random colors and add them to the colors array
    private void AddColors()
    {
        colors = new Color[COLOR_COUNT];

        for (int index = 0; index < COLOR_COUNT; index++)
        {
            System.Random random = new System.Random();
            float r = (float)random.NextDouble();
            float g = (float)random.NextDouble();
            float b = (float)random.Next(5) / 10;

            colors[index] = new Color(r, g, b);
        }
    }
}
