using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTiles : MonoBehaviour
{
    //[HideInInspector]
    public GameObject previousTile { get; private set; }
    public GameObject nextTile { get; private set; }
    public bool linked { get; set; }

    public void Link(GameObject nextTile)
    {
        this.nextTile = nextTile;
        nextTile.GetComponent<LinkTiles>().previousTile = this.gameObject;

        Color newColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        GetComponent<SpriteRenderer>().color = newColor;
        nextTile.GetComponent<SpriteRenderer>().color = newColor;

        linked = true;
        nextTile.GetComponent<LinkTiles>().linked = true;
    }

    public int Length()
    {
        int length = 0;
        GameObject tile = this.gameObject;
        while (tile != null)
        {
            length++;
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        return length;
    }
}
