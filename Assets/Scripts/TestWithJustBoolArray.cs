using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;
using System;

public class TestWithJustBoolArray : MonoBehaviour
{
    [SerializeField] private GameObject[] moveTiles;

    private bool[][] locations = new bool[ROW_COUNT][];
    private GameObject[][] tileObjects = new GameObject[ROW_COUNT][];

    // Constants
    private const int ROW_COUNT = 11;
    private const int COLUMN_COUNT = 10;
    private const int START_COUNT = 3;
    private const float WAIT_TIME = 0.5f;

    // Set play area
    void Start()
    {
        for (int i = 0; i < ROW_COUNT; i++)
        {
            locations[i] = MakeEmptyLocations();
            tileObjects[i] = MakeEmptyTileObject();
        }

        for (int i = 0; i < START_COUNT; i++)
        {
            MakeNextSet();
            ConnectTiles(0);
            for (int j = i; j > -1; j--)
            {
                PushRowUp(j);
            }
        }
    }

    // Adjust play area when button is pressed
    void Update()
    {
        if (Input.anyKeyDown && PlayerPrefs.GetInt("MovingBlockSet") == 0 && !locations[ROW_COUNT - 1].Contains(true))
        {
            for (int i = ROW_COUNT - 2; i > -1; i--)
            {
                PushRowUp(i);
            }

            MakeNextSet();
            ConnectTiles(0);

            StartCoroutine(WaitForMovement());
        }
    }

    // Create a row of all false values for the locations array
    private bool[] MakeEmptyLocations()
    {
        bool[] newLocationSet = new bool[COLUMN_COUNT];
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            newLocationSet[i] = false;
        }
        return newLocationSet;
    }

    // Create a row of all null values for the tileObjects array
    private GameObject[] MakeEmptyTileObject()
    {
        GameObject[] newTileSet = new GameObject[COLUMN_COUNT];
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            newTileSet[i] = null;
        }
        return newTileSet;
    }

    // Connect tiles of a certain row
    private void ConnectTiles(int row)
    {
        float randomChance;
        int length = 1;
        GameObject tile;
        GameObject nextTile;

        for (int i = 0; i < COLUMN_COUNT - 1; i++)
        {
            if (length == 4) // No tiles with a lengther > 4
            {
                length = 1;
                continue;
            }

            if (!locations[row][i] || !locations[row][i + 1]) // Skip any null tiles
            {
                length = 1;
                continue;
            }

            randomChance = UnityEngine.Random.Range(0f, 1f);

            /*if (randomChance < 0.25f) continue; // 75% chance ot combine if not linked at all
            if (randomChance < 0.50f && length == 2) continue; // 50% chance to combine*/

            if (randomChance < length * 0.25f) continue;

            tile = tileObjects[row][i];
            nextTile = tileObjects[row][i + 1];
            tile.GetComponent<Linked>().Link(nextTile); // Link
            length++;
        }
    }

    // Generate the next set of location values
    private void MakeNextSet()
    {
        bool[] nextSet = new bool[COLUMN_COUNT];
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            nextSet[i] = true;
        }

        for (int i = 0; i < 4; i++)
        {
            int randomPosition = UnityEngine.Random.Range(0, COLUMN_COUNT);
            nextSet[randomPosition] = false;
        }

        locations[0] = nextSet;

        DisplayTiles(nextSet);
    }

    // Push the current row up by 1 and set the current row to its empty value
    private void PushRowUp(int row)
    {
        foreach (GameObject tile in tileObjects[row])
        {
            if (tile == null) continue;
            tile.GetComponent<Move>().MoveUp(row);
        }

        // Push
        locations[row + 1] = locations[row];
        tileObjects[row + 1] = tileObjects[row];

        // Empty
        locations[row] = MakeEmptyLocations();
        tileObjects[row] = MakeEmptyTileObject();
    }

    // Create a tile object at all true values in locations array
    private void DisplayTiles(bool[] nextSet)
    {
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            if (nextSet[i])
            {
                Vector3 position = new Vector3(i, 0, 0);
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                GameObject tileObject = Instantiate(moveTiles[0], position, rotation);
                tileObjects[0][i] = tileObject;
            }
        }
    }

    private bool CheckTilesUnder(int row)
    {
        bool moved = false;

        if (row - 1 < 0) return false; // Out of bounds
        if (!locations[row].Contains(true)) return false; // Empty

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            GameObject currentTile = tileObjects[row][column];
            if (currentTile == null)
            {
                continue;
            }

            Linked linked = currentTile.GetComponent<Linked>();
            if (linked.GetNextTile() == null && linked.GetPreviousTile() == null && tileObjects[row - 1][column] == null)
            {
                MoveTileDown(row, column);
                moved = true;
                continue;
            }

            if (CheckConnectedTilesUnder(row, column))
            {
                MoveTileDown(row, column);
                moved = true;
            }
        }

        return moved;
    }

    private bool CheckConnectedTilesUnder(int row, int column)
    {
        bool canMove = true;
        int startColumn = column;
        GameObject currentTile = tileObjects[row][column];
        while (currentTile != null)
        {
            if (locations[row - 1][column]) canMove = false;
            currentTile = currentTile.GetComponent<Linked>().GetNextTile();
            column++;
        }

        column = startColumn;
        currentTile = tileObjects[row][column];

        while (currentTile != null)
        {
            if (locations[row - 1][column] && tileObjects[row - 1][column] != currentTile) canMove = false;
            currentTile = currentTile.GetComponent<Linked>().GetPreviousTile();
            column--;
        }

        return canMove;
    }

    private void MoveTileDown(int row, int column)
    {
        GameObject currentTile = tileObjects[row][column];

        currentTile.GetComponent<Move>().MoveDown(row);
        locations[row - 1][column] = true;
        tileObjects[row - 1][column] = currentTile;

        locations[row][column] = false;
        tileObjects[row][column] = null;
    }

    private bool CheckRowComplete(int row)
    {
        if (locations[row].Contains(false)) return false;

        GameObject tile;
        Linked linked;
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            tile = tileObjects[row][i];
            linked = tile.GetComponent<Linked>();
            if (linked.GetNextTile() != null) linked.SetNextTile(null);
            if (linked.GetPreviousTile() != null) linked.SetPreviousTile(null);
            Destroy(tileObjects[row][i]);
        }

        locations[row] = MakeEmptyLocations();
        tileObjects[row] = MakeEmptyTileObject();

        return true;
    }

    // Wait for some time before checking tiles below
    IEnumerator WaitForMovement()
    {
        yield return new WaitForSeconds(WAIT_TIME);

        int row = 0;
        while (row != ROW_COUNT - 1)
        {
            if (CheckTilesUnder(row) == true) row = 0;
            row++;
        }

        StartCoroutine(CheckRowComplete());
    }

    IEnumerator CheckRowComplete()
    {
        yield return new WaitForSeconds(WAIT_TIME);

        for (int i = 0; i < ROW_COUNT - 1; i++)
        {
            if (CheckRowComplete(i))
            {
                StartCoroutine(WaitForMovement());
            }
        }
    }

    public bool[] GetLocationRow(int row)
    {
        return locations[row];
    }

    public GameObject[] GetTileObjectRow(int row)
    {
        return tileObjects[row];
    }

    public void SetLocation(int row, int column, bool value)
    {
        locations[row][column] = value;
    }

    public void SetTileObject(int row, int column, GameObject tile)
    {
        tileObjects[row][column] = tile;
    }
}
