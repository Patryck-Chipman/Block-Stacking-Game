using System.Collections;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // TODO Add comments and make row destroy after being complete


    [SerializeField] private GameObject[] _moveTiles;

    public bool[][] locations { get; set; }
    public GameObject[][] tileObjects { get; set; }

    // Constants
    private const int ROW_COUNT = 11;
    private const int COLUMN_COUNT = 10;

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

    private bool[] MakeSet()
    {
        const int START_COUNT = 3;
        bool[] nextSet = new bool[COLUMN_COUNT];
        System.Random random = new System.Random();

        for (int column = 0; column < COLUMN_COUNT; column++)
        {
            nextSet[column] = true;
        }

        for (int i = 0; i < START_COUNT; i++)
        {
            int removePosition = random.Next(COLUMN_COUNT);
            nextSet[removePosition] = false;
        }

        return nextSet;
    }

    private void LinkBottomRow()
    {
        const float LINK_NUM = 0.5f;
        int length = 0;
        System.Random random = new System.Random();

        for (int index = 0; index < COLUMN_COUNT - 1; index++)
        {
            GameObject tile = tileObjects[0][index];
            GameObject nextTile = tileObjects[0][index + 1];

            if (length == 3)
            {
                length = 0;
                continue;
            }

            if (tile == null || nextTile == null)
            {
                length = 0;
                continue;
            }

            if (random.NextDouble() < LINK_NUM)
            {
                length = 0;
                continue;
            }

            tile.GetComponent<LinkTiles>().Link(nextTile);
            length++;
        }
    }

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

    private void MoveRowsUp(int row)
    {
        if (row == 0)
        {
            bool[] newSet = MakeSet();
            locations[0] = newSet;
            DisplayTiles(newSet);
            LinkBottomRow();
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
                //item.GetComponent<MoveTile>().MoveAll(false, row);
                item.GetComponent<MoveTile>().Move(position);
            }
            catch (System.Exception ex) { }
        }

        MoveRowsUp(row - 1);
    }

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

    private bool CheckTile(int tileRow, int tileColumn)
    {
        if (locations[tileRow - 1][tileColumn]) return false;
        return true;
    }

    private IEnumerator Fall(bool pushUp)
    {
        int row = 1;
        while (row < ROW_COUNT)
        {
            if (CheckRowBelow(row)) row = 0;
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

    public void MoveUp()
    {
        MoveRowsUp(ROW_COUNT - 1);
    }

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
