using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScoreTile : MonoBehaviour
{
    private float _originalB = -1;
    private float _newB;

    /// <summary>
    /// Method <c>ChangeScoreMultiplier</c> adjusts the score multiplier for this tile
    /// </summary>
    /// <param name="scoreMultiplier"></param> the new score multiplier
    public void ChangeScoreMultiplier(float scoreMultiplier)
    {
        GetComponent<TileScore>().scoreMultiplier = scoreMultiplier;

        StartCoroutine(ChangeColor());
    }

    // Rotate between two colors
    private IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(0.2f);

        float r = GetComponent<SpriteRenderer>().color.r;
        float g = GetComponent<SpriteRenderer>().color.g;
        float b = GetComponent<SpriteRenderer>().color.b;

        if (_originalB < 0)
        {
             _originalB = b;
             _newB = b + 0.5f;
        }

        while (GetComponent<SpriteRenderer>().color.b <= _newB)
        {
            b += 0.05f;
            GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
            yield return new WaitForSeconds(0.1f);
        }

        while (GetComponent<SpriteRenderer>().color.b >= _originalB)
        {
            b -= 0.05f;
            GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(ChangeColor());
    }
}
