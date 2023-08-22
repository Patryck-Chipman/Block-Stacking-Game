using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChooser : MonoBehaviour
{
    /// <summary>
    /// Field <c>sprites</c> is the list of all sprites the object can pick from
    /// </summary>
    public Sprite[] sprites;

    private LinkTiles _linked;
    private GameObject _nextTile;
    private GameObject _previousTile;
    private TileColor _colors;

    private const int COLOR_COUNT = 5;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = PickSprite();
        GetComponent<SpriteRenderer>().color = PickColor();
    }

    private Color PickColor()
    {
        _colors = Camera.main.GetComponent<TileColor>();
        UpdateVariables();

        if (_previousTile == null)
        {
            System.Random random = new System.Random();
            return _colors.colors[random.Next(COLOR_COUNT)];
        }

        return _previousTile.GetComponent<SpriteRenderer>().color;
    }

    private void UpdateVariables()
    {
        _linked = GetComponent<LinkTiles>();
        _nextTile = _linked.nextTile;
        _previousTile = _linked.previousTile;
    }

    /// <summary>
    /// Method <c>PickSprite</c> chooses the proper sprite based on linkage
    /// </summary>
    /// <returns>The sprite that should be applied</returns>
    public Sprite PickSprite()
    {
        UpdateVariables();

        if (_nextTile != null && _previousTile != null) return sprites[0];
        if (_nextTile != null) return sprites[1];
        if (_previousTile != null) return sprites[2];
        return sprites[3];
    }
}
