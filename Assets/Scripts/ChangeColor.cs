using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private float _originalB = -1;
    private float _newB;
    private IEnumerator _coroutine;

    // Start is called before the first frame update
    /*void Start()
    {
        _coroutine = RotateColor();
        StartCoroutine(_coroutine);
    }*/

    /// <summary>
    /// Method <c>FadeToGray</c> changes the color of the tile to gray
    /// </summary>
    public void ChangeToGray()
    {
        Debug.Log("Fading");
        try
        {
            StopCoroutine(_coroutine);
        }
        catch (System.Exception ex) { }

        _coroutine = FadeToGray();
        StartCoroutine(_coroutine);
    }

    /// <summary>
    /// Method <c>Rotate</c> rotates the tiles color in a pulsating manor
    /// </summary>
    public void Rotate()
    {
        _coroutine = RotateColor();
        StartCoroutine(_coroutine);
    }

    // Rotate between two colors
    private IEnumerator RotateColor()
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

        _coroutine = RotateColor();
        StartCoroutine(_coroutine);
    }

    // Change the tile to gray
    private IEnumerator FadeToGray()
    {
        yield return new WaitForSeconds(0.2f);

        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
