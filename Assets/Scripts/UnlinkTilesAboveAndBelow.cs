using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlinkTilesAboveAndBelow : MonoBehaviour
{
    private GameObject[][] _tileObjects;
    private GameObject _nextTile;
    private GameObject _previousTile;

    /// <summary>
    /// Method <c>UnlinkTiles</c> causes the tiles above and below to be unlinked
    /// </summary>
    /// <param name="row"></param> the row of the current tile
    /// <param name="column"></param> the column of the current tile
    public void UnlinkTiles(int row, int column)
    {
        _tileObjects = Camera.main.GetComponent<BoardController>().tileObjects;

        // Above tile
        try
        {
            GameObject tileAbove = _tileObjects[row + 1][column];
            SetNextAndPreviousTiles(tileAbove);
            tileAbove.GetComponent<LinkTiles>().Unlink();
            ChangeNextAndPreviousSprites();
            tileAbove.GetComponent<SpriteRenderer>().sprite = tileAbove.GetComponent<SpriteChooser>().PickSprite();
        }
        catch (System.Exception ex) { }

        // Below tile
        try
        {
            GameObject tileBelow = _tileObjects[row - 1][column];
            SetNextAndPreviousTiles(tileBelow);
            tileBelow.GetComponent<LinkTiles>().Unlink();
            ChangeNextAndPreviousSprites();
            tileBelow.GetComponent<SpriteRenderer>().sprite = tileBelow.GetComponent<SpriteChooser>().PickSprite();
        }
        catch (System.Exception ex) { }
    }

    private void SetNextAndPreviousTiles(GameObject currentTile)
    {
        _nextTile = currentTile.GetComponent<LinkTiles>().nextTile;
        _previousTile = currentTile.GetComponent<LinkTiles>().previousTile;
    }

    private void ChangeNextAndPreviousSprites()
    {
        // Next tile
        try
        {
            _nextTile.GetComponent<SpriteRenderer>().sprite = _nextTile.GetComponent<SpriteChooser>().PickSprite();
        }
        catch (System.Exception ex) { }

        // Previous tile
        try
        {
            _previousTile.GetComponent<SpriteRenderer>().sprite = _previousTile.GetComponent<SpriteChooser>().PickSprite();
        }
        catch (System.Exception ex) { }
    }
}
