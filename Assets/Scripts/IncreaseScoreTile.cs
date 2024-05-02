using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IncreaseScoreTile : MonoBehaviour
{
    /// <summary>
    /// Method <c>ChangeScoreMultiplier</c> adjusts the score multiplier for this tile
    /// </summary>
    /// <param name="scoreMultiplier"></param> the new score multiplier
    public void ChangeScoreMultiplier()
    {
        PlayerPrefs.SetFloat("ScoreMuliplier", PlayerPrefs.GetFloat("ScoreMuliplier") + 0.5f);
    }
}
