using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDestroyTile : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        GetComponent<TileScore>().scoreMultiplier = 1.5f; 
    }
}
