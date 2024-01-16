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
        int displayedHighScore = Int32.Parse(transform.GetComponent<TextMeshProUGUI>().text);
        if (highscore > displayedHighScore) DisplayScore();
    }

    /// <summary>
    /// Method <c>DisplayScore</c> displays the highscore
    /// </summary>
    public void DisplayScore()
    {
        transform.GetComponent<TextMeshProUGUI>().text = highscore.ToString();
    }
}
