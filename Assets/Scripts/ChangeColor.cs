using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private float _originalB = -1;
    private float _newB;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RotateColor());
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

        StartCoroutine(RotateColor());
    }
}
