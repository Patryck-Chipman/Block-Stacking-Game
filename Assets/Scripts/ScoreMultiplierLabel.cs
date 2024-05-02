using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreMultiplierLabel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Check());
    }

    private void UpdateLabel()
    {
        string formatted = PlayerPrefs.GetFloat("ScoreMultiplier").ToString("N");
        GetComponent<TextMeshProUGUI>().text = formatted + "x";
    }

    private IEnumerator Check()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateLabel();
        StartCoroutine(Check());
    }
}
