using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelPercentageLabel : MonoBehaviour
{
    /// <summary>
    /// Method <c>DisplayPercentage</c> displays the percentage of the level bar
    /// </summary>
    /// <param name="percentage"></param> the total percentage to show
    public void DisplayPercentage(float percentage)
    {
        GetComponent<TextMeshProUGUI>().text = string.Format("{0}%", percentage.ToString("N0"));
        if (percentage > 50) GetComponent<TextMeshProUGUI>().color = Color.black;
    }
}
