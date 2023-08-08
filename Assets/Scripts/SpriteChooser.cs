using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChooser : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _sprites;

    private LinkTiles _linked;
    private GameObject _nextTile;
    private GameObject _previousTile;
    private TileColor _colors;

    private const int COLOR_COUNT = 5;

    void Start()
    {
        _linked = GetComponent<LinkTiles>();
        _nextTile = _linked.nextTile;
        _previousTile = _linked.previousTile;

        GetComponent<SpriteRenderer>().sprite = PickSprite();
        GetComponent<SpriteRenderer>().color = PickColor();
    }

    private Sprite PickSprite()
    {
        if (_nextTile != null && _previousTile != null) return _sprites[0];
        if (_nextTile != null) return _sprites[1];
        if (_previousTile != null) return _sprites[2];
        return _sprites[3];
    }

    private Color PickColor()
    {
        _colors = Camera.main.GetComponent<TileColor>();

        if (_previousTile == null)
        {
            System.Random random = new System.Random();
            return _colors.colors[random.Next(COLOR_COUNT)];
        }

        return _previousTile.GetComponent<SpriteRenderer>().color;
    }
}
