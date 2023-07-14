using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linked : MonoBehaviour
{
    public GameObject previousTile = null;
    public GameObject nextTile = null;

    // Link two tiles together
    public void Link(GameObject nextTile)
    {
        this.nextTile = nextTile;
        nextTile.GetComponent<Linked>().SetPreviousTile(this.gameObject);
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a = 1;
        GetComponent<SpriteRenderer>().color = newColor;
        nextTile.GetComponent<SpriteRenderer>().color = newColor;
    }

    // Set the previous tile
    public void SetPreviousTile(GameObject previousTile)
    {
        this.previousTile = previousTile;
    }

    // Set the next tile
    public void SetNextTile(GameObject nextTile)
    {
        this.nextTile = nextTile;
    }

    // Return the next tile
    public GameObject GetNextTile()
    {
        return nextTile;
    }

    // Return previous tile
    public GameObject GetPreviousTile()
    {
        return previousTile;
    }
}
