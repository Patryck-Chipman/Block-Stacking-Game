using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    private int highscore;

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("High Score");
        DisplayScore();
    }

    private void Update()
    {
        highscore = PlayerPrefs.GetInt("High Score");
        DisplayScore();
    }

    /// <summary>
    /// Method <c>DisplayScore</c> displays the highscore
    /// </summary>
    public void DisplayScore()
    {
        string formatted = GetComponent<IntegerLabel>().format(highscore);
        transform.GetComponent<TextMeshProUGUI>().text = formatted;
    }
}
