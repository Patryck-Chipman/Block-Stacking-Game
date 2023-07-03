using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFreely : MonoBehaviour
{
    private bool follow = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetFollow()) transform.position = GetMousePosition();
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane + 9.7f;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.y = transform.position.y;
        return mousePosition;
    }

    private bool GetFollow()
    {
        return follow;
    }

    private void SetFollow(bool changeTo)
    {
        int binaryChange = changeTo ? 1 : 0; // 1 if true, 0 if false
        PlayerPrefs.SetInt("MovingBlockSet", binaryChange);
        follow = changeTo;
    }

    private void OnMouseDown()
    {
        if (!GetFollow() && PlayerPrefs.GetInt("MovingBlockSet") == 0)
        {
            SetFollow(true);
        }
        else
        {
            SetFollow(false);
            transform.position = new Vector3(Mathf.Floor(transform.position.x) - 0.5f, transform.position.y, 0);
        }
    }
}
