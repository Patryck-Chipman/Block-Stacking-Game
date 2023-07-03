using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public Tilemap map;
    //public Vector3 newPosition;

    [SerializeField] private Tile[] setTiles;
    private Vector3 newPosition;
    private float speed;
    private bool falling;

    private void Start()
    {
        speed = 0.25f * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (falling)
            Fall();
    }

    // Make the tile fall
    private void Fall()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed);
        if (transform.position == newPosition)
        {
            map.SetTile(Vector3Int.FloorToInt(transform.position), setTiles[0]);
            falling = false;
        }
    }

    // Set falling to true
    public void MakeFall()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y - 1, 0);
        falling = true;
    }
}
