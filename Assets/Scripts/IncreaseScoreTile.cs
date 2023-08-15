using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScoreTile : MonoBehaviour
{
    // TODO: Figure out why tiles automatically choose one color (0.5 0.5, 0.5)

    private Color _color;
    private Color _fadeToColor;

    public void ChangeScoreMultiplier(float scoreMultiplier)
    {
        GetComponent<TileScore>().scoreMultiplier = scoreMultiplier;
        _color = GetComponent<SpriteRenderer>().color;

        _fadeToColor = _color;
        _fadeToColor.b += 0.5f;

        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        Color currentColor = _color;

        while (currentColor.b <= _fadeToColor.b)
        {
            currentColor.b += 0.05f;
            GetComponent<SpriteRenderer>().color = currentColor;
            yield return new WaitForSeconds(0.1f);
        }

        while (currentColor.b >= _color.b)
        {
            currentColor.b -= 0.05f;
            GetComponent<SpriteRenderer>().color = currentColor;
            yield return new WaitForSeconds(0.1f);
        }

        //StartCoroutine(ChangeColor());
    }
}
