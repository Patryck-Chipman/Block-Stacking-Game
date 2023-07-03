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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        if (follow) transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnMouseOver()
    {
        follow = true;
    }
}
