using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScore : MonoBehaviour
{
    public float scoreMultiplier { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplier = 1;
    }
}
