using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private HighScore _highScore;

    private void Start()
    {
        PlayerPrefs.SetInt("BaseScore", 5);
        PlayerPrefs.SetInt("Score", 0);
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

        if (tile.GetComponent<LinkTiles>().nextTile != null || tile.GetComponent<LinkTiles>().previousTile != null)
            score *= 1.3f;

        return Mathf.RoundToInt(score);
    }

    /// <summary>
    /// Method <c>AddScore</c> adds the given score to the players total score count for the current run
    /// </summary>
    /// <param name="scoreToAdd">the score to be added</param>
    public void AddScore(int scoreToAdd)
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + scoreToAdd);

        if (PlayerPrefs.GetInt("Score") >= PlayerPrefs.GetInt("ScoreIncreaseThreshold"))
        {
            PlayerPrefs.SetInt("BaseScore", Mathf.RoundToInt(PlayerPrefs.GetInt("BaseScore") * 1.2f));
            PlayerPrefs.SetInt("ScoreIncreaseThreshold", PlayerPrefs.GetInt("ScoreIncreaseThreshold") * 2);
        }

        if (PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("High Score"))
        {
            PlayerPrefs.SetInt("High Score", PlayerPrefs.GetInt("Score"));
            _highScore.DisplayScore();
        }

        DisplayScore();
    }
}
