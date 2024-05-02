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
    [SerializeField] private GameOver _gameOver;
    [SerializeField] private SoundPlayer _soundPlayer;
    [SerializeField] private AudioClip _rowCompleteSound;

    // Constants
    private const int ROW_COUNT = 8;
    private const int COLUMN_COUNT = 8;
    private const int BASE_SCORE = 100;

    // Global
    private int score = BASE_SCORE;
    private float score_multiplier = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        PlayerPrefs.SetInt("Moved", 0);
        locations = new bool[ROW_COUNT][];
        tileObjects = new GameObject[ROW_COUNT][];

        for (int i = 0; i < ROW_COUNT; i++)
        {
            locations[i] = MakeEmptyLocations();
            tileObjects[i] = MakeEmptyTileObject();
        }

        for (int i = 0; i < 2; i++)
        {
            MoveRowsUp(ROW_COUNT - 1);
        }

        StartCoroutine(Fall(true));
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

    /// <summary>
    /// Method <c>MakeEmptyLocations</c> create a row of all false values for the locations array
    /// </summary>
    /// <returns></returns>
    private bool[] MakeEmptyLocations()
    {
        bool[] newLocationSet = new bool[COLUMN_COUNT];
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            newLocationSet[i] = false;
        }
        return newLocationSet;
    }

    /// <summary>
    /// Method <c>MakeEmptyTileObject</c> create a row of all null values for the tileObjects array
    /// </summary>
    /// <returns></returns>
    private GameObject[] MakeEmptyTileObject()
    {
        GameObject[] newTileSet = new GameObject[COLUMN_COUNT];
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            newTileSet[i] = null;
        }
        return newTileSet;
    }

    /// <summary>
    /// Method <c>MakeSet</c> creates the next bottom array
    /// </summary>
    /// <returns>the new set</returns>
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

    /// <summary>
    /// Method <c>LinkBottomRow</c> randomly links tiles in the bottom row
    /// </summary>
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

    /// <summary>
    /// Method <c>DisplayTiles</c> instatiates tiles based on their position in the array
    /// </summary>
    /// <param name="set">bool[] of a row. true means a tile is there, false otherwise</param>
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

    /// <summary>
    /// Method <c>EmptyTiles</c> makes some tile spaces empty
    /// </summary>
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

    /// <summary>
    /// Method <c>MakeProperLength</c> ensures that all tiles are the proper length (not longer than 4)
    /// </summary>
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

    /// <summary>
    /// Method <c>MakePowerTiles</c> converts some tiles into powerful tiles
    /// </summary>
    private void MakePowerTiles()
    {
        System.Random random = new System.Random();

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            GameObject tile = tileObjects[0][column];

            if (tile == null) continue;

            // Multi-row destroy tile or UnlinkTile
            if (random.NextDouble() > 0.99)
            {
                if (random.NextDouble() > 0.5)
                    MultiDestroyTile(tile);
                else UnLinkAboveAndBelowTile(tile);
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

    /// <summary>
    /// Method <c>IncreaseScoreTile</c> converts the given tile into a special tile
    /// </summary>
    /// <param name="tile"></param>
    private void IncreaseScoreTile(GameObject tile, float scoreMultiplier)
    {
        GameObject orginTile = tile;

        tile.AddComponent<IncreaseScoreTile>();
        tile.GetComponent<ChangeColor>().Rotate();
        tile.GetComponent<IncreaseScoreTile>().ChangeScoreMultiplier();
        tile = tile.GetComponent<LinkTiles>().nextTile;

        while (tile != null)
        {
            tile.AddComponent<IncreaseScoreTile>();
            tile.GetComponent<ChangeColor>().Rotate();
            //tile.GetComponent<IncreaseScoreTile>().ChangeScoreMultiplier(scoreMultiplier);
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = orginTile.GetComponent<LinkTiles>().previousTile;

        while (tile != null)
        {
            tile.AddComponent<IncreaseScoreTile>();
            tile.GetComponent<ChangeColor>().Rotate();
            //tile.GetComponent<IncreaseScoreTile>().ChangeScoreMultiplier(scoreMultiplier);
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    /// <summary>
    /// Method <c>MultiDestroyTile</c> converts the given tile into a special tile
    /// </summary>
    /// <param name="tile"></param>
    private void MultiDestroyTile(GameObject tile)
    {
        GameObject orginTile = tile;

        while (tile != null)
        {
            tile.AddComponent<MultiDestroyTile>();
            tile.GetComponent<ChangeColor>().Rotate();
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = orginTile.GetComponent<LinkTiles>().previousTile;

        while (tile != null)
        {
            tile.AddComponent<MultiDestroyTile>();
            tile.GetComponent<ChangeColor>().Rotate();
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    /// <summary>
    /// Method <c>UnLinkAboveAndBelowTile</c> converts the given tile into a special tile
    /// </summary>
    /// <param name="tile"></param>
    private void UnLinkAboveAndBelowTile(GameObject tile)
    {
        GameObject orginTile = tile;

        while (tile != null)
        {
            tile.AddComponent<UnlinkTilesAboveAndBelow>();
            tile.GetComponent<ChangeColor>().Rotate();
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = orginTile.GetComponent<LinkTiles>().previousTile;

        while (tile != null)
        {
            tile.AddComponent<UnlinkTilesAboveAndBelow>();
            tile.GetComponent<ChangeColor>().Rotate();
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    /// <summary>
    /// Method <c>MoveRowsUp</c> moves the given row up by one
    /// </summary>
    /// <param name="row"></param>
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

    /// <summary>
    /// Method <c>CheckRowBelow</c> checks to see if the row below and moves tiles accordingly
    /// </summary>
    /// <param name="row"></param>
    /// <returns>Whether tiles have been moved or not</returns>
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

    /// <summary>
    /// Method <c>MoveTilesDown</c> moves the tile at the given row and column down
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
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

    /// <summary>
    /// Method <c>CheckMultiLengthTile</c> check if the tile at the row and column is multiple tiles wide
    /// </summary>
    /// <param name="tileRow"></param>
    /// <param name="tileColumn"></param>
    /// <returns>true if the tile is multiple wide, false otherwise</returns>
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

    /// <summary>
    /// Method <c>CheckTile</c> checks if their is a tile at the given row(-1) and column
    /// </summary>
    /// <param name="tileRow"></param>
    /// <param name="tileColumn"></param>
    /// <returns>true if there is not a tile at the position, flase otherwise</returns>
    private bool CheckTile(int tileRow, int tileColumn)
    {
        if (locations[tileRow - 1][tileColumn]) return false;
        return true;
    }

    /// <summary>
    /// Method <c>RowComplete</c> determines whether the row is complete or not
    /// </summary>
    /// <param name="row">the row to check</param>
    /// <returns>true if complete, false otherwise</returns>
    private bool RowComplete(int row)
    {
        return !locations[row].Contains(false);
    }

    /// <summary>
    /// Method <c>DestroyRow</c> this one is self explanitory
    /// </summary>
    /// <param name="row"></param>
    private void DestroyRow(int row)
    {
        if (row < 0 || row > ROW_COUNT) return;

        bool destroyAboveAndBelow = false;
        float totalScore = 0;
        totalScore += _scoreboard.GenerateScore();

        _soundPlayer.PlaySound(_rowCompleteSound);

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            if (!locations[row][column]) continue;

            if (tileObjects[row][column].TryGetComponent<IncreaseScoreTile>(out IncreaseScoreTile _))
                totalScore *= 1.1f;

            if (tileObjects[row][column].TryGetComponent<MultiDestroyTile>(out MultiDestroyTile _))
                destroyAboveAndBelow = true;

            if (tileObjects[row][column].TryGetComponent<UnlinkTilesAboveAndBelow>(out UnlinkTilesAboveAndBelow tile))
                tile.UnlinkTiles(row, column);

            locations[row][column] = false;
            Destroy(tileObjects[row][column]);
            tileObjects[row][column] = null;
        }

        _scoreboard.AddScore((int)totalScore);
        _scoreboard.CreateScorePopup((int)totalScore);

        _fuelBar.IncreaseFuel(1000);

        PlayerPrefs.SetInt("Rows Destroyed", PlayerPrefs.GetInt("Rows Destroyed") + 1);

        if (destroyAboveAndBelow) DestroyRow(row + 1);
    }

    /// <summary>
    /// Method <c>Fall</c> causes the tiles to fall then be pushed up a row
    /// </summary>
    /// <param name="pushUp">boolean that determines whethe ror not to go up</param>
    /// <returns></returns>
    private IEnumerator Fall(bool pushUp)
    {
        int closeCalls = PlayerPrefs.GetInt("Close Calls");
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

        // Runs twice for some reason
        if (closeCalls != PlayerPrefs.GetInt("Close Calls")) yield break;
        if (locations[ROW_COUNT - 2].Contains(true)) PlayerPrefs.SetInt("Close Calls", PlayerPrefs.GetInt("Close Calls") + 1);
        if (locations[ROW_COUNT - 1].Contains(true)) _gameOver.StopGame();
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
