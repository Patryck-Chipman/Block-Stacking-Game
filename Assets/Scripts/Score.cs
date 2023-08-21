using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private HighScore _highScore;
    [SerializeField] private LevelProgressBar _progressBar;

    private void Start()
    {
        PlayerPrefs.SetInt("BaseScore", 8);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("ScoreIncreaseThreshold", 300);
        DisplayScore();
    }

    // Displays the score in the text component
    private void DisplayScore()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Score").ToString();
    }

    /// <summary>
    /// Method <c>GenerateScore</c> generates the score the given tile.
    /// </summary>
    /// <param name="tile">the tile for which score should be calculated.</param>
    /// <returns>The calculated score</returns>
    public int GenerateScore(GameObject tile)
    {
        float score = PlayerPrefs.GetInt("BaseScore");

        if (tile == null) return Mathf.RoundToInt(score);

        return Mathf.RoundToInt(score * tile.GetComponent<TileScore>().scoreMultiplier);
    }

    /// <summary>
    /// Method <c>AddScore</c> adds the given score to the players total score count for the current run
    /// </summary>
    /// <param name="scoreToAdd">the score to be added</param>
    public void AddScore(int scoreToAdd)
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + scoreToAdd);

        int score = PlayerPrefs.GetInt("Score");

        if (score >= PlayerPrefs.GetInt("ScoreIncreaseThreshold"))
        {
            PlayerPrefs.SetInt("BaseScore", Mathf.RoundToInt(PlayerPrefs.GetInt("BaseScore") * 1.5f));
            PlayerPrefs.SetInt("ScoreIncreaseThreshold", (int)(PlayerPrefs.GetInt("ScoreIncreaseThreshold") * 2.5f));
        }

        if (score > PlayerPrefs.GetInt("High Score"))
        {
            PlayerPrefs.SetInt("High Score", score);
            _highScore.DisplayScore();
        }

        _progressBar.CalculateFillPercent(score);

        DisplayScore();
    }
}
