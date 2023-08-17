using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlinkTilesAboveAndBelow : MonoBehaviour
{
    private GameObject[][] _tileObjects;

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
            tileAbove.GetComponent<LinkTiles>().Unlink();
            tileAbove.GetComponent<SpriteRenderer>().sprite = tileAbove.GetComponent<SpriteChooser>().sprites[3];
        }
        catch (System.Exception ex) { }

        // Below tile
        try
        {
            GameObject tileBelow = _tileObjects[row - 1][column];
            tileBelow.GetComponent<LinkTiles>().Unlink();
            tileBelow.GetComponent<SpriteRenderer>().sprite = tileBelow.GetComponent<SpriteChooser>().sprites[3];
        }
        catch (System.Exception ex) { }
    }
}
