using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTile : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    /// <summary>
    /// Field <c>follow</c> is whether the object is following the pointer (mouse/finger)
    /// </summary>
    public bool follow { get; private set; }
    /// <summary>
    /// Field <c>lastPosition</c> is the last position the tile was before being picked up
    /// </summary>
    public Vector2 lastPosition { get; private set; }
    /// <summary>
    /// Field <c>distance</c> is how far from the epicenter (mouse position) this tile should be
    /// </summary>
    public int distance { get; private set; }
    /// <summary>
    /// Field <c>pointerPosition</c> is the currentPosition of the pointer
    /// </summary>
    public Vector2 pointerPosition;

    private Vector2 _position;
    private Vector2 _newPosition;
    private bool _moving;

    private const float MOVE_TIME = 0.25f;

    private void Start()
    {
        PlayerPrefs.SetInt("IsMoving", 0);
        distance = 0;
    }

    private void FixedUpdate()
    {
        // Move object towards new position
        if (_moving)
        {
            _position = transform.position;
        }

        // Make the object no longer move
        if (_position == _newPosition)
        {
            if (PlayerPrefs.GetInt("Moved") == 1)
                PlayerPrefs.SetInt("Moved", 2);
            //PlayerPrefs.SetInt("IsMoving", 0);
            GetComponent<SpriteRenderer>().sortingOrder = 2;
            _moving = false;
        }

        // Have the object follow the mouse
        if (follow)
        {
            Vector2 newPosition = new Vector2(pointerPosition.x, transform.position.y);
            newPosition.x += distance;
            transform.position = newPosition;
            
        }
    }

    // Detect mouse down events
    /*private void OnMouseDown()
    {
        if (PlayerPrefs.GetInt("Game Over") == 1) return;

        // Move everything to new position (or old position)
        if (follow)
        {
            if (!ValidMove())
            {
                MoveAll(true, 0);
                return;
            }

            PlayerPrefs.SetInt("Moved", 1);
            MoveAll(false, 0);
            return;
        }

        if (PlayerPrefs.GetInt("IsMoving") == 1) return;

        PlayerPrefs.SetInt("IsMoving", 1);
        MakeAllFollow();
    }*/

    /// <summary>
    /// Method <c>OnPointerDown</c> computes what to do when the user presses their pointer (mouse/finger)
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        if (PlayerPrefs.GetInt("Game Over") == 1) return;

        // Move everything to new position (or old position)
        if (follow)
        {
            if (!ValidMove())
            {
                //MoveAll(true, 0);
                return;
            }

            PlayerPrefs.SetInt("Moved", 1);
            MoveAll(false, 0);
            return;
        }

        if (PlayerPrefs.GetInt("IsMoving") == 1) return;

        PlayerPrefs.SetInt("IsMoving", 1);
        MakeAllFollow();
    }

    /// <summary>
    /// Method <c>OnPointerUp</c> computes what to do when the user lifts thier pointer(mouse/finger)
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerUp(PointerEventData data)
    {
        if (PlayerPrefs.GetInt("Game Over") == 1) return;

        if (follow)
        {
            if (!ValidMove())
            {
                //MoveAll(true, 0);
                return;
            }

            PlayerPrefs.SetInt("Moved", 1);
            MoveAll(false, 0);
            return;
        }
    }

    /// <summary>
    /// Method <c>OnDrag</c> computes what to do while the user is dragging their pointer (mouse/finger)
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        if (follow)
        {
            pointerPosition = Camera.main.ScreenToWorldPoint(data.position);

            GameObject tile = GetComponent<LinkTiles>().nextTile;
            while (tile != null)
            {
                tile.GetComponent<MoveTile>().pointerPosition = pointerPosition;
                tile = tile.GetComponent<LinkTiles>().nextTile;
            }
            tile = GetComponent<LinkTiles>().previousTile;
            while (tile != null)
            {
                tile.GetComponent<MoveTile>().pointerPosition = pointerPosition;
                tile = tile.GetComponent<LinkTiles>().previousTile;
            }
        }
    }

    /// <summary>
    /// Method <c>MakePosition</c> creates a rounded position vector
    /// </summary>
    /// <returns>Vector2 rounded position</returns>
    private Vector2 MakePosition()
    {
        Vector2 position = transform.position;

        position.x = Mathf.RoundToInt(position.x);

        return position;
    }

    // Return mouse position but keep y as current y value
    /*private Vector3 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.y = transform.position.y;
        return mousePosition;
    }*/


    /// <summary>
    /// Method <c>ValidMove</c> determines whether a move is avlid or not
    /// </summary>
    /// <returns>true if the move is unobstructed, false otherwise</returns>
    private bool ValidMove()
    {
        GameObject tile = this.gameObject;
        BoardController boardControl = Camera.main.GetComponent<BoardController>();
        Vector2 newPosition = MakePosition();
        int column = Mathf.RoundToInt(newPosition.x);
        int originalColumn = column;
        int row = Mathf.RoundToInt(newPosition.y);

        if (SamePosition()) return false;

        // Check rows at right tiles
        while (tile != null)
        {
            SetOldValues(row, column);
            if (boardControl.locations[row][column])
            {
                Debug.Log("Case 1: Move is invalid at - " + row + " " + column);
                return false;
            }
            tile = tile.GetComponent<LinkTiles>().nextTile;
            column++;
        }

        tile = this.gameObject.GetComponent<LinkTiles>().previousTile;
        column = originalColumn - 1;

        // Check rows at left tiles
        while (tile != null)
        {
            SetOldValues(row, column);
            if (boardControl.locations[row][column])
            {
                Debug.Log("Case 2: Move is invalid at - " + row + " " + column);
                return false;
            }
            tile = tile.GetComponent<LinkTiles>().previousTile;
            column--;
        }

        Debug.Log("Case 3: Move is valid");
        return true;
    }

    /// <summary>
    /// Method <c>SamePosition</c> determines if the position the tile is at is the same as its previous one
    /// </summary>
    /// <returns>true if the position is the same, false if not</returns>
    private bool SamePosition()
    {
        if (Mathf.RoundToInt(transform.position.x) == Mathf.RoundToInt(lastPosition.x)) return true;

        return false;
    }

    /// <summary>
    /// Method <c>MakeAllFollow</c> causes all linked tiles to follow the current one
    /// </summary>
    private void MakeAllFollow()
    {
        GameObject tile = this.gameObject;
        int newDistance = distance;

        // Make tiles to the right follow
        while (tile != null)
        {
            tile.GetComponent<MoveTile>().Follow(newDistance);
            tile = tile.GetComponent<LinkTiles>().nextTile;
            newDistance++;
        }

        tile = this.gameObject.GetComponent<LinkTiles>().previousTile;
        newDistance = distance - 1;

        // Make tiles to the left follow
        while (tile != null)
        {
            tile.GetComponent<MoveTile>().Follow(newDistance);
            tile = tile.GetComponent<LinkTiles>().previousTile;
            newDistance--;
        }
    }

    /// <summary>
    /// Method <c>Move</c> moves the tile to its new position
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    private IEnumerator Move(Vector2 startPosition, Vector2 newPosition)
    {
        for (float time = 0; time < 1; time += Time.deltaTime / MOVE_TIME)
        {
            transform.position = Vector2.Lerp(startPosition, newPosition, time);
            yield return null;
        }
    }

    /// <summary>
    /// Method <c>Move</c> begins the moving process for the tiles
    /// </summary>
    /// <param name="newPosition"></param>
    public void Move(Vector2 newPosition)
    {
        //lastPosition = transform.position;
        int newX = Mathf.RoundToInt(newPosition.x);
        int newY = Mathf.RoundToInt(newPosition.y);

        newPosition.x = newX;
        newPosition.y = newY;

        SetNewValues(newY, newX);
        SetNewValues(newY, newX);

        _moving = true;
        _newPosition = newPosition;

        // Floats are weird man
        if (newPosition.y == Mathf.RoundToInt(transform.position.y))
        {
            transform.position = newPosition;
            return;
        }

        StartCoroutine(Move(transform.position, newPosition));
    }

    /// <summary>
    /// Method <c>MoveAll</c> moves all tiles connect to the current one to a new position
    /// </summary>
    /// <param name="back">boolean <c>back</c> determines whether the current set of tiles is moving to their original position</param>
    /// <param name="row">int <c>row</c> is the row which the tiles are moving to</param>
    public void MoveAll(bool back, int row)
    {
        BoardController boardControl = Camera.main.GetComponent<BoardController>();
        GameObject tile = this.gameObject;
        MoveTile moveTile;
        Vector2 newPosition;

        // Move right tiles
        while (tile != null)
        {
            moveTile = tile.GetComponent<MoveTile>();

            if (back)
            {
                newPosition = moveTile.lastPosition;

                int column = Mathf.RoundToInt(newPosition.x);
                boardControl.locations[row][column] = true;
                boardControl.tileObjects[row][column] = tile;
            }
            else
                newPosition = moveTile.MakePosition();

            if (row != 0)
                newPosition.y = row;

            moveTile.follow = false;
            moveTile.distance = 0;
            moveTile.Move(newPosition);
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = this.gameObject;

        // Move left tiles
        while (tile != null)
        {
            moveTile = tile.GetComponent<MoveTile>();

            if (back)
            {
                newPosition = moveTile.lastPosition;

                int column = Mathf.RoundToInt(newPosition.x);
                boardControl.locations[row][column] = true;
                boardControl.tileObjects[row][column] = tile;
            }
            else
                newPosition = moveTile.MakePosition();

            if (row != 0)
                newPosition.y = row;

            moveTile.follow = false;
            moveTile.distance = 0;
            moveTile.Move(newPosition);
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }

        PlayerPrefs.SetInt("IsMoving", 0);
    }

    /// <summary>
    /// Method <c>SetNewValues</c> updates BoardController array values at the new position
    /// </summary>
    /// <param name="newRow"></param> the row which will now have the tile
    /// <param name="newColumn"></param> the column which will now have the tile
    public void SetNewValues(int newRow, int newColumn)
    {
        //SetLocationAndTile();
        BoardController boardControl = Camera.main.GetComponent<BoardController>();
        boardControl.locations[newRow][newColumn] = true;

        boardControl.tileObjects[newRow][newColumn] = gameObject;
    }

    /// <summary>
    /// Method <c>SetOldValues</c> updates the locations and tileObject arrays at the position of
    /// the object to false and null
    /// </summary>
    public void SetOldValues(int row, int column)
    {
        GameObject tile = this.gameObject;
        BoardController boardControl = Camera.main.GetComponent<BoardController>();
        int currentColumn = Mathf.RoundToInt(lastPosition.x);
        int originalColumn = currentColumn;
        int currentRow = Mathf.RoundToInt(lastPosition.y);

        // Right tiles
        while (tile != null)
        {
            boardControl.locations[currentRow][currentColumn] = false;
            boardControl.tileObjects[currentRow][currentColumn] = null;
            currentColumn++;
            tile = tile.GetComponent<LinkTiles>().nextTile;
        }

        tile = this.gameObject;
        currentColumn = originalColumn;

        // Left tiles
        while (tile != null)
        {
            boardControl.locations[currentRow][currentColumn] = false;
            boardControl.tileObjects[currentRow][currentColumn] = null;
            currentColumn--;
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    /// <summary>
    /// Method <c>Follow</c> begins the follow process for this object
    /// </summary>
    /// <param name="distance"></param>
    public void Follow(int distance)
    {
        this.distance = distance;
        lastPosition = transform.position;
        GetComponent<SpriteRenderer>().sortingOrder = 3;
        follow = true;
    }
}
