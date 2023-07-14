using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    private bool moving;
    private bool follow = false;
    private Vector3 moveTo;
    private Vector3 startPosition;

    private const float MOVE_SPEED = 5;
    private const int COLUMN_COUNT = 10;

    void Update()
    {
        if (transform.position == moveTo) moving = false;
        if (moving)
            transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime * MOVE_SPEED);

        if (GetFollow())
        {
            MakeLinkedFollow(0);
            //transform.position = GetMousePosition();
        }
    }

    // Move tile up one row
    public void MoveUp(int row)
    {
        Vector3 position = transform.position;
        position.y = row + 1;
        moveTo = position;
        moving = true;
        //transform.position = position;
    }

    // Move tile down one row
    public void MoveDown(int row)
    {
        Vector3 position = transform.position;
        position.y = row - 1;
        moveTo = position;
        moving = true;
        //transform.position = position;
    }

    public bool GetFollow()
    {
        return follow;
    }


    // Set follow
    private void SetFollow(bool changeTo)
    {
        follow = changeTo; 
    }

    private void OnMouseDown()
    {
        if (GetFollow())
        {
            SetFollow(false);
            PlaceTiles(0);
            PlayerPrefs.SetInt("MovingBlockSet", 0);
            return;
        }

        SetStartPosition();
        SetFollow(true);
        SetArrayValues(false, null);
        PlayerPrefs.SetInt("MovingBlockSet", 1);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane + 9.7f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.y = transform.position.y;
        if (mousePosition.x >= COLUMN_COUNT || mousePosition.x < 0) return transform.position;
        return mousePosition;
    }

    private void MakeLinkedFollow(int placement)
    {
        Linked linked = GetComponent<Linked>();
        GameObject previousTile = linked.GetPreviousTile();
        GameObject nextTile = linked.GetNextTile();
        Vector3 mousePosition = GetMousePosition();
        SetArrayValues(false, null);

        mousePosition.x += placement;
        transform.position = mousePosition;

        if (previousTile != null && placement <= 0) previousTile.GetComponent<Move>().MakeLinkedFollow(placement - 1);
        if (nextTile != null && placement >= 0) nextTile.GetComponent<Move>().MakeLinkedFollow(placement + 1);
    }

    private void PlaceTiles(int placement)
    {
        Linked linked = GetComponent<Linked>();
        GameObject previousTile = linked.GetPreviousTile();
        GameObject nextTile = linked.GetNextTile();
        Vector3 position = transform.position;
        
        position.x = Mathf.RoundToInt(position.x);
        transform.position = position;

        SetArrayValues(true, this.gameObject);

        //PlayerPrefs.SetInt("MovingBlockSet", -1); when object has to go back to original spot

        if (previousTile != null && placement <= 0) previousTile.GetComponent<Move>().PlaceTiles(placement - 1);
        if (nextTile != null && placement >= 0) nextTile.GetComponent<Move>().PlaceTiles(placement + 1);
    }

    private void SetArrayValues(bool locationValue, GameObject tileObject)
    {
        Vector3 position = transform.position;
        var settersAndGetters = Camera.main.GetComponent<TestWithJustBoolArray>();
        int row = Mathf.RoundToInt(position.y);
        int column = Mathf.RoundToInt(position.x);

        settersAndGetters.SetLocation(row, column, locationValue);
        settersAndGetters.SetTileObject(row, column, tileObject);
    }

    private void SetStartPosition()
    {
        startPosition = transform.position;
    }
}