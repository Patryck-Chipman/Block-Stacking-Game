using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMapTest : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private Tile[] setTiles;
    [SerializeField] private GameObject[] moveTiles;

    private List<List<GameObject>> tileObjectList;
    private Spots spots;

    // Start is called before the first frame update
    void Start()
    {
        map.CompressBounds();

        spots = GameObject.Find("Spots Taken").GetComponent<Spots>();

        tileObjectList = new List<List<GameObject>>();

        for (int i = 0; i < 10; i++)
        {
            tileObjectList.Add(new List<GameObject>());
            for (int j = 0; j < 6; j++)
            {
                tileObjectList[i].Add(null);
            }
        }

        MakeNextSet();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && PlayerPrefs.GetInt("MovingBlockSet") == 0)
        {
            for (int i = -5; i < 5; i++)
            {
                if (map.GetTile(new Vector3Int(i, 2, 0)).Equals(setTiles[1])) return;
            }
            for (int i = 1; i > -4; i--)
            {
                MoveSetUp(i);
            }
            spots.PushAllUp();
            MakeNextSet();
            for (int i = -2; i < 3; i++)
            {
                //CheckTileBelow(i);
            }
        }
    }

    // Create the next set of tiles
    private void MakeNextSet()
    {
        bool[] nextSet = new bool[10];

        // Set all tiles true
        for (int i = 0; i < nextSet.Length; i++)
        {
            nextSet[i] = true;
        }

        // Randonly select 2 tiles to be false (can be the same tile)
        for (int i = 0; i < 2; i++)
        {
            int randomPosition = Random.Range(0, nextSet.Length);
            nextSet[randomPosition] = false;
        }

        // Make the tiles visible
        for (int i = 0; i < nextSet.Length; i++)
        {
            if (nextSet[i] == true)
            {
                map.SetTile(new Vector3Int(i - 5, -3, 0), setTiles[1]);
                GameObject newTile = Instantiate(moveTiles[0], new Vector3(i - 4.5f, -2.5f), Quaternion.Euler(0, 0, 0));
                newTile.GetComponent<MoveDown>().map = map;
                tileObjectList[i][0] = newTile;
            }
        }

        spots.Insert(nextSet);
    }

    // Move the set up one row
    private void MoveSetUp(int currentRow)
    {
        for (int i = -5; i < 5; i++)
        {
            Vector3Int currentPosition = CreateCurrentPosition(i, currentRow);
            var currentTile = CreateCurrentTile(currentPosition);

            if (currentTile.Equals(setTiles[0])) continue;

            map.SetTile(currentPosition, setTiles[0]);
            Vector3Int newPosition = new Vector3Int(i, currentRow + 1, 0);
            map.SetTile(newPosition, currentTile);

            var currentColumnList = tileObjectList[i + 5];
            var currentTileObject = currentColumnList[currentRow + 3];

            currentColumnList[currentRow + 4] = currentTileObject;
            currentTileObject.transform.position = new Vector3(currentTileObject.transform.position.x, currentTileObject.transform.position.y + 1, 0);
        }
    }

    // Determine if the tile below can have a tile drop down
    private void CheckTileBelow(int currentRow)
    {
        for (int i = -5; i < 5; i++)
        {
            Vector3Int currentPosition = CreateCurrentPosition(i, currentRow);
            var currentTile = CreateCurrentTile(currentPosition);

            Vector3Int newPosition = new Vector3Int(i, currentRow - 1, 0);

            if (currentTile.Equals(setTiles[0])) continue;

            if (map.GetTile(newPosition).Equals(setTiles[0]))
            {
                map.SetTile(currentPosition, setTiles[0]);
                MoveTileDown(currentPosition, newPosition);
                var currentColumnList = tileObjectList[i + 5];
                var currentTileObject = currentColumnList[currentRow + 3];

                currentColumnList[currentRow + 2] = currentTileObject;
                currentTileObject.GetComponent<MoveDown>().MakeFall();
            }
        }
    }

    // Return the current position
    private Vector3Int CreateCurrentPosition(int currentColumn, int currentRow)
    {
        return new Vector3Int(currentColumn, currentRow, 0);
    }

    // Return the current Tile
    private TileBase CreateCurrentTile(Vector3Int currentPosition)
    {
        return map.GetTile(currentPosition);
    }

    // Make a tile fall to the previous row
    private void MoveTileDown(Vector3Int currentPosition, Vector3Int newPosition)
    {
        //map.SetTile(currentPosition, setTiles[0]);

        /*var tileMove = Instantiate(moveTiles[0], currentPosition, Quaternion.Euler(0, 0, 0));
        tileMove.transform.position = new Vector3(currentPosition.x + 0.5f, currentPosition.y + 0.5f, 0);
        tileMove.GetComponent<MoveDown>().map = map;
        tileMove.GetComponent<MoveDown>().newPosition = new Vector3(newPosition.x + 0.5f, newPosition.y + 0.5f, 0);*/
        //tileMove.GetComponent<MoveDown>().MakeFall();
    }
}
