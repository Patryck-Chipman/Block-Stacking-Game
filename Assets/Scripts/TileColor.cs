using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColor : MonoBehaviour
{
    public Color[] colors { get; private set; }

    private const int COLOR_COUNT = 5;

    private void Start()
    {
        AddColors();
    }

    private void AddColors()
    {
        colors = new Color[COLOR_COUNT];

        for (int index = 0; index < COLOR_COUNT; index++)
        {
            System.Random random = new System.Random();
            float r = (float)random.NextDouble();
            float g = (float)random.NextDouble();
            float b = (float)random.NextDouble();

            colors[index] = new Color(r, g, b);
        }
    }
}
