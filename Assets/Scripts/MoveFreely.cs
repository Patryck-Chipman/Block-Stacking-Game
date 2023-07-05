using UnityEngine;

public class MoveFreely : MonoBehaviour
{
    private Vector3 startPosition;
    private Spots spots;
    private bool follow = false;

    void Start()
    {
        spots = GameObject.Find("Spots Taken").GetComponent<Spots>();
    }

    // Update block position
    void Update()
    {
        if (GetFollow()) transform.position = GetMousePosition();
    }

    // Return the current mouse position
    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane + 9.7f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.y = transform.position.y;
        return mousePosition;
    }

    // Return follow
    private bool GetFollow()
    {
        return follow;
    }

    // Set follow
    private void SetFollow(bool changeTo)
    {
        follow = changeTo;
    }

    // Return the return value of spots.Move with object parameters
    private bool GetSpotOccupied(int currentRow, int startColumn, int newColumn)
    {
        return spots.Move(currentRow, startColumn, newColumn);
    }

    // Return startPosition
    private Vector3 GetStartPosition()
    {
        return startPosition;
    }

    // Set startPosition based on the objects position
    private void SetStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    // Detect when the mouse has been pressed while hovering this object
    private void OnMouseDown()
    {
        // Case 1: No object is currently being moved, move this object
        if (!GetFollow() && PlayerPrefs.GetInt("MovingBlockSet") <= 0)
        {
            SetStartPosition(transform.position);
            SetFollow(true);
            PlayerPrefs.SetInt("MovingBlockSet", 1);
            return;
        }

        Vector3 startPosition = GetStartPosition();
        int startRow = (int)(startPosition.y + 2.5f);
        int startColumn = (int)(startPosition.x + 4.5f);
        int newColumn = (int)(Mathf.Floor(transform.position.x) + 5);

        // Case 2: Object is over an unoccupied space, make this space the objects new position
        if (GetSpotOccupied(startRow, startColumn, newColumn))
        {
            SetFollow(false);
            PlayerPrefs.SetInt("MovingBlockSet", 0);
            transform.position = new Vector3(newColumn - 4.5f, transform.position.y, 0);
            return;
        }

        // Case 3: Object is over an occupied spot, move object back to start
        transform.position = startPosition;
        PlayerPrefs.SetInt("MovingBlockSet", -1);
        SetFollow(false);
    }
}
