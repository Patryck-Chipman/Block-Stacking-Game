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

        GetComponent<TileScore>().scoreMultiplier = 1.5f;
        nextTile.GetComponent<TileScore>().scoreMultiplier = 1.5f;
    }

    /// <summary>
    /// Method <c>Unlink</c> unlinks this tile from the ones directly next to it
    /// </summary>
    public void Unlink()
    {
        GetComponent<TileScore>().scoreMultiplier = 1f;

        try { UnlinkNext(); }
        catch (System.Exception ex) { }

        try { UnlinkPrevious(); }
        catch (System.Exception ex) { }
    }

    /// <summary>
    /// Method <c>UnlinkNext</c> unlinks this tile from the next tile
    /// </summary>
    public void UnlinkNext()
    {
        LinkTiles linked = nextTile.GetComponent<LinkTiles>();

        if (linked.nextTile == null) nextTile.GetComponent<TileScore>().scoreMultiplier = 1f;

        linked.previousTile = null;
        nextTile = null;
    }

    /// <summary>
    /// Method <c>UnlinkPrevious</c> unlinks this tile from the previous tile
    /// </summary>
    public void UnlinkPrevious()
    {
        LinkTiles linked = previousTile.GetComponent<LinkTiles>();

        if (linked.previousTile == null) previousTile.GetComponent<TileScore>().scoreMultiplier = 1f;

        previousTile.GetComponent<LinkTiles>().nextTile = null;
        previousTile = null;
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
