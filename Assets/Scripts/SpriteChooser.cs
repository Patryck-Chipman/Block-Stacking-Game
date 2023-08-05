using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChooser : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = PickSprite();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private Sprite PickSprite()
    {
        LinkTiles linked = GetComponent<LinkTiles>();
        GameObject nextTile = linked.nextTile;
        GameObject previousTile = linked.previousTile;

        Color newColor = GetComponent<SpriteRenderer>().color;
        //newColor.a = 0.5f;
        GetComponent<SpriteRenderer>().color = newColor;

        if (nextTile != null && previousTile != null) return sprites[0];
        if (nextTile != null) return sprites[1];
        if (previousTile != null) return sprites[2];
        return sprites[3];
    }
}
