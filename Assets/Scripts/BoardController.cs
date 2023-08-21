using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardController : MonoBehaviour
{
    /// <summary>
    /// Field <c>locations</c> is the boolean array of rows and columns (true: occupied)
    /// </summary>
    public bool[][] locations;
    /// <summary>
    /// Field <c>tileObjects</c> is the GameObject array of rows cand columns
    /// </summary>
    public GameObject[][] tileObjects;

    [SerializeField] private GameObject[] _moveTiles;
    [SerializeField] private Score _scoreboard;
    [SerializeField] private FuelBar _fuelBar;

    // Constants
    private const int ROW_COUNT = 11;
    private const int COLUMN_COUNT = 9;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Moved", 0);
        PlayerPrefs.SetInt("IsMoving", 0);

        locations = new bool[ROW_COUNT][];
        tileObjects = new GameObject[ROW_COUNT][];

        for (int i = 0; i < ROW_COUNT; i++)
        {
            locations[i] = MakeEmptyLocations();
            tileObjects[i] = MakeEmptyTileObject();
        }

        for (int i = 0; i < 3; i++)
        {
            MoveRowsUp(ROW_COUNT - 1);
        }

        StartCoroutine(Fall(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Moved") == 2)
        {
            PlayerPrefs.SetInt("Moved", 0);
            StartCoroutine(Fall(true));
            _fuelBar.IncreaseFuel(500);
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

    // Create the next (bottom) set of the array
    private bool[] MakeSet()
    {
        bool[] nextSet = new bool[COLUMN_COUNT];
        System.Random random = new System.Random();

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            nextSet[column] = true;
        }

        return nextSet;
    }

    // Randomly choose whether or not to link the tiles on the bottom row
    private void LinkBottomRow()
    {
        int length = 0;
        System.Random random = new System.Random();

        for (int index = 0; index < COLUMN_COUNT - 1; index++)
        {
            GameObject tile = tileObjects[0][index];
            GameObject nextTile = tileObjects[0][index + 1];

            tile.GetComponent<LinkTiles>().Link(nextTile);
            length++;
        }
    }

    // Instantiate the tiles based on their position in the array
    private void DisplayTiles(bool[] set)
    {
        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            if (set[column])
            {
                Vector3 position = new Vector3(column, 0, 0);
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                GameObject tileObject = Instantiate(_moveTiles[0], position, rotation);
                tileObjects[0][column] = tileObject;
            }
        }
    }

    // Make some tiloe spaces empty (can overlap)
    private void EmptyTiles()
    {
        System.Random random = new System.Random();

        for (int count = 0; count < 3; count++)
        {
            int index = random.Next(COLUMN_COUNT);
            GameObject tile = tileObjects[0][index];

            try
            {
                tile.GetComponent<LinkTiles>().Unlink();
            }
            catch (System.Exception ex){ }

            Destroy(tile);
            locations[0][index] = false;
            tileObjects[0][index] = null;
        }
    }

    // Ensure no tiles are longer than 4 tiles
    private void MakeProperLength()
    {
        int length = 0;

        for (int index = 0; index < COLUMN_COUNT - 1; index++)
        {
            GameObject tile = tileObjects[0][index];
            GameObject nextTile = tileObjects[0][index + 1];

            if (tile == null || nextTile == null)
            {
                length = 0;
                continue;
            }

            if (length == 3)
            {
                tile.GetComponent<LinkTiles>().UnlinkNext();
                length = 0;
                continue;
            }

            length++;
        }
    }

    // Create powerUp tiles
    private void MakePowerTiles()
    {
        System.Random random = new System.Random();

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            GameObject tile = tileObjects[0][column];

            if (tile == null) continue;

            // Multi-row destroy tile
            if (random.NextDouble() > 0.95)
            {
                MultiDestroyTile(tile);
                return;
            }

            if (random.NextDouble() > 0.93)
            {
                UnLinkAboveAndBelowTile(tile);
                return;
            }

            // Double score tile
            if (random.NextDouble() > 0.90)
            {
                IncreaseScoreTile(tile, 2.5f);
                return;
            }
        }
    }

    // Change tiles to increased score tiles
    private void IncreaseScoreTile(GameObject tile, float scoreMultiplier)
    {
        GameObject orginTile = tile;

        while (tile != null)
        {
            tile.AddComponent<IncreaseScoreTile>();
            tile.AddComponent<ChangeColor>();
            tile.GetComponent<IncreaseScoreTile>().ChangeScoreMultiplier(scoreMultiplier);
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = orginTile.GetComponent<LinkTiles>().previousTile;

        while (tile != null)
        {
            tile.AddComponent<IncreaseScoreTile>();
            tile.AddComponent<ChangeColor>();
            tile.GetComponent<IncreaseScoreTile>().ChangeScoreMultiplier(scoreMultiplier);
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    // Convert a tile into a multi-row destroy tile
    private void MultiDestroyTile(GameObject tile)
    {
        GameObject orginTile = tile;

        while (tile != null)
        {
            tile.AddComponent<MultiDestroyTile>();
            tile.AddComponent<ChangeColor>();
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = orginTile.GetComponent<LinkTiles>().previousTile;

        while (tile != null)
        {
            tile.AddComponent<MultiDestroyTile>();
            tile.AddComponent<ChangeColor>();
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    // Convert tiles to a tile which unlinks tiles above and below it
    private void UnLinkAboveAndBelowTile(GameObject tile)
    {
        GameObject orginTile = tile;

        while (tile != null)
        {
            tile.AddComponent<UnlinkTilesAboveAndBelow>();
            tile.AddComponent<ChangeColor>();
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = orginTile.GetComponent<LinkTiles>().previousTile;

        while (tile != null)
        {
            tile.AddComponent<UnlinkTilesAboveAndBelow>();
            tile.AddComponent<ChangeColor>();
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    // Move every row up by one
    private void MoveRowsUp(int row)
    {
        if (row == 0)
        {
            bool[] newSet = MakeSet();
            locations[0] = newSet;
            DisplayTiles(newSet);
            LinkBottomRow();
            EmptyTiles();
            MakeProperLength();
            MakePowerTiles();
            return;
        }
        
        locations[row] = locations[row - 1];
        tileObjects[row] = tileObjects[row - 1];

        locations[row - 1] = MakeEmptyLocations();
        tileObjects[row - 1] = MakeEmptyTileObject();

        foreach (var item in tileObjects[row])
        {
            try
            {
                Vector2 position = item.transform.position;
                position.y = row;
                item.GetComponent<MoveTile>().Move(position);
            }
            catch (System.Exception ex) { } // Tile is null
        }

        MoveRowsUp(row - 1);
    }

    // Decide if the row below each tile is good to be moved to, and move if so
    private bool CheckRowBelow(int row)
    {
        if (row == ROW_COUNT) return false;

        bool[] rowArray = locations[row];
        bool[] rowBelowArray = locations[row - 1];
        bool moved = false;
        int column = 0;

        while (column < COLUMN_COUNT)
        {
            GameObject tile = tileObjects[row][column];

            if (tile == null)
            {
                column++;
                continue;
            }

            if (tile.GetComponent<LinkTiles>().linked)
            {
                if (CheckMultiLengthTile(row, column))
                {
                    moved = true;
                    while (tile != null)
                    {
                        MoveTilesDown(row, column);
                        column++;
                        tile = tile.GetComponent<LinkTiles>().nextTile;
                    }
                    continue;
                }

                column += tile.GetComponent<LinkTiles>().Length();
                continue;
            }

            if (CheckTile(row, column))
            {
                moved = true;
                MoveTilesDown(row, column);
            }

            column++;
        }

        return moved;
    }

    // Move the tile at the row and column down by one
    private void MoveTilesDown(int row, int column)
    {
        int rowBelow = row - 1;

        GameObject tile = tileObjects[row][column];
        Vector2 newPosition = new Vector2(column, rowBelow);

        locations[rowBelow][column] = true;
        locations[row][column] = false;
        tileObjects[rowBelow][column] = tile;
        tileObjects[row][column] = null;

        tile.GetComponent<MoveTile>().Move(newPosition);
    }

    // Determines if the tile is multilengthed
    private bool CheckMultiLengthTile(int tileRow, int tileColumn)
    {
        GameObject tile = tileObjects[tileRow][tileColumn];
        while (tile != null)
        {
            if (!CheckTile(tileRow, tileColumn)) return false;
            tile = tile.GetComponent<LinkTiles>().nextTile;
            tileColumn++;
        }

        return true;
    }

    // Determines if there is a tile at the row below
    private bool CheckTile(int tileRow, int tileColumn)
    {
        if (locations[tileRow - 1][tileColumn]) return false;
        return true;
    }

    // Determine if the row has been complete
    private bool RowComplete(int row)
    {
        return !locations[row].Contains(false);
    }

    // Destroy the given row
    private void DestroyRow(int row)
    {
        if (row < 0 || row > ROW_COUNT) return;

        bool destroyAboveAndBelow = false;

        int totalScore = 0;

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            totalScore += _scoreboard.GenerateScore(tileObjects[row][column]);

            if (!locations[row][column]) continue;

            if (tileObjects[row][column].TryGetComponent<MultiDestroyTile>(out MultiDestroyTile _))
                destroyAboveAndBelow = true;

            if (tileObjects[row][column].TryGetComponent<UnlinkTilesAboveAndBelow>(out UnlinkTilesAboveAndBelow tile))
                tile.UnlinkTiles(row, column);

            locations[row][column] = false;
            Destroy(tileObjects[row][column]);
            tileObjects[row][column] = null;
        }

        _scoreboard.AddScore(totalScore);

        _fuelBar.IncreaseFuel(1000);

        if (destroyAboveAndBelow) DestroyRow(row + 1);
    }

    // Makes tiles fall then go up
    private IEnumerator Fall(bool pushUp)
    {
        int row = 1;
        while (row < ROW_COUNT)
        {
            if (CheckRowBelow(row)) row = 0;

            if (RowComplete(row))
            {
                yield return new WaitForSeconds(0.5f);
                DestroyRow(row);
                StartCoroutine(Fall(true));
                yield break;
            }

            row++;
        }

        yield return new WaitForSeconds(0.25f);

        if (pushUp)
        {
            MoveRowsUp(ROW_COUNT - 1);
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(Fall(false));
        }

    }

    /// <summary>
    /// Method <c>MoveUp</c> moves all tiles up by one row
    /// </summary>
    public void MoveUp()
    {
        MoveRowsUp(ROW_COUNT - 1);
    }

    /// <summary>
    /// Method <c>Print</c> outputs each row of the locations array
    /// </summary>
    public void Print()
    {
        foreach (var row in locations)
        {
            string outString = "{ ";
            foreach (var column in row)
            {
                outString += column + ", ";
            }
            outString += "}";
            Debug.Log(outString);
        }
    }
}
