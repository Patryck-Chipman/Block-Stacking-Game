using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    //TODO

    public bool follow { get; private set; }
    public Vector2 lastPosition { get; private set; }
    public int distance { get; private set; }

    private Vector2 _position;
    private Vector2 _newPosition;
    private bool _moving;

    private const float MOVE_TIME = 0.25f;

    private void Start()
    {
        distance = 0;
    }

    // Begin moving process
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

        Debug.Log(newPosition.y + " " + transform.position.y);

        // Floats are weird man
        if (newPosition.y == Mathf.RoundToInt(transform.position.y))
        {
            transform.position = newPosition;
            return;
        }

        StartCoroutine(Move(transform.position, newPosition));
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
            PlayerPrefs.SetInt("IsMoving", 0);
            GetComponent<SpriteRenderer>().sortingOrder = 2;
            _moving = false;
        }

        // Have the object follow the mouse
        if (follow)
        {
            Vector2 newPosition = GetMousePosition();
            newPosition.x += distance;
            transform.position = newPosition;
        }
    }

    // Detect mouse down events
    private void OnMouseDown()
    {
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

        if (PlayerPrefs.GetInt("IsMoving") != 0) return;

        PlayerPrefs.SetInt("IsMoving", 1);
        MakeAllFollow();
    }

    // Return rounded position
    private Vector2 MakePosition()
    {
        Vector2 position = transform.position;

        position.x = Mathf.RoundToInt(position.x);

        return position;
    }

    // Return mouse position but keep y as current y value
    private Vector3 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.y = transform.position.y;
        return mousePosition;
    }


    // Return whether the designated move is valid
    private bool ValidMove()
    {

        GameObject tile = this.gameObject;
        BoardController boardControl = Camera.main.GetComponent<BoardController>();
        Vector2 newPosition = MakePosition();
        int column = Mathf.RoundToInt(newPosition.x);
        int originalColumn = column;
        int row = Mathf.RoundToInt(newPosition.y);

        // Check rows at right tiles
        while (tile != null)
        {
            if (boardControl.locations[row][column]) return false;
            tile = tile.GetComponent<LinkTiles>().nextTile;
            SetOldValues(row, column);
            column++;
        }

        tile = this.gameObject;
        column = originalColumn;

        // Check rows at left tiles
        while (tile != null)
        {
            if (boardControl.locations[row][column]) return false;
            tile = tile.GetComponent<LinkTiles>().previousTile;
            SetOldValues(row, column);
            column--;
        }

        return true;
    }

    // Move this tile as well as the ones connected to it
    public void MoveAll(bool back, int row)
    {
        GameObject tile = this.gameObject;
        MoveTile moveTile;
        Vector2 newPosition;

        // Move right tiles
        while (tile != null)
        {
            moveTile = tile.GetComponent<MoveTile>();

            if (back)
                newPosition = moveTile.lastPosition;
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
                newPosition = moveTile.lastPosition;
            else
                newPosition = moveTile.MakePosition();

            if (row != 0)
                newPosition.y = row;

            moveTile.follow = false;
            moveTile.distance = 0;
            moveTile.Move(newPosition);
            tile = tile.GetComponent<LinkTiles>().previousTile;
        }
    }

    // Make all tiles connected to this one follow the mouse
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
            Debug.Log(newDistance);
        }

        tile = this.gameObject;
        newDistance = distance;

        // Make tiles to the left follow
        while (tile != null)
        {
            tile.GetComponent<MoveTile>().Follow(newDistance);
            tile = tile.GetComponent<LinkTiles>().previousTile;
            newDistance--;
        }
    }

    // Move tile
    private IEnumerator Move(Vector2 startPosition, Vector2 newPosition)
    {
        for (float time = 0; time < 1; time += Time.deltaTime / MOVE_TIME)
        {
            transform.position = Vector2.Lerp(startPosition, newPosition, time);
            yield return null;
        }
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
