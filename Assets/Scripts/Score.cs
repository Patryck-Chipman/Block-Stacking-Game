using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Score : MonoBehaviour
{
    [SerializeField] private HighScore _highScore;
    [SerializeField] private LevelProgressBar _progressBar;
    [SerializeField] private GameObject _scorePopupPrefab;

    private void Start()
    {
        PlayerPrefs.SetInt("BaseScore", 8);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("ScoreIncreaseThreshold", 1000);
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
    /// Method <c>CreateScorePopup</c> creates a textbox on the screen that displays the score earned
    /// </summary>
    /// <param name="score">Score to display</param>
    public void CreateScorePopup(int score)
    {
        var randomX = Random.Range(140, 900);
        var randomY = Random.Range(1200, 1400);
        Vector2 labelPosition = new Vector2(randomX, randomY);
        Quaternion labelRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(-45, 45));
        GameObject scorePopup = Instantiate(_scorePopupPrefab, labelPosition, labelRotation);
        scorePopup.transform.SetParent(GameObject.Find("Canvas").transform);
        scorePopup.GetComponent<TextMeshProUGUI>().text = score.ToString();
        PlayerPrefs.SetInt("Total Score", PlayerPrefs.GetInt("Total Score") + score);
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
            PlayerPrefs.SetInt("ScoreIncreaseThreshold", (int)(PlayerPrefs.GetInt("ScoreIncreaseThreshold") * 1.5f));
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
