using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    private Color color;
    private float a = 1;

    private void Start()
    {
        color = GetComponent<TextMeshProUGUI>().color;
        StartCoroutine(fadeAway());
    }

    private IEnumerator fadeAway()
    {
        while (a > 0)
        {
            yield return new WaitForSeconds(0.025f);
            a -= 0.05f;
            color.a = a;
            GetComponent<TextMeshProUGUI>().color = color;
        }

        Destroy(gameObject);
    }
}
