using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Statistic : MonoBehaviour
{
    [SerializeField] protected string statName;

    private void Start()
    {
        DisplayScore();
    }

    /// <summary>
    /// Method <c>DisplayScore</c> displays the high score of the player
    /// </summary>
    public void DisplayScore()
    {
        GetComponent<TextMeshProUGUI>().text = statName + ": " + PlayerPrefs.GetInt(statName).ToString();
    }
}
