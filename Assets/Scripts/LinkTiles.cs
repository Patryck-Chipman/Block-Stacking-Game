using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTiles : MonoBehaviour
{
    //[HideInInspector]
    public GameObject previousTile { get; private set; }
    public GameObject nextTile { get; private set; }
    public bool linked { get; set; }

    /// <summary>
    /// Method <c>Link</c> links the current tile with the given tile
    /// </summary>
    /// <param name="nextTile">The tile to link to the current tile</param>
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

    /// <summary>
    /// Method <c>Length</c> returns the length of the tile chain
    /// </summary>
    /// <returns>Tile chain length</returns>
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
