using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("BaseScore", 5);
    }

    public int GenerateScore(GameObject tile)
    {
        float score = PlayerPrefs.GetInt("BaseScore");

        if (tile.GetComponent<LinkTiles>().nextTile != null || tile.GetComponent<LinkTiles>().previousTile != null)
            score *= 1.3f;

        return Mathf.RoundToInt(score);
    }

    public void AddScore(int scoreToAdd)
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + scoreToAdd);

        if (PlayerPrefs.GetInt("Score") >= PlayerPrefs.GetInt("ScoreIncreaseThreshold"))
        {
            PlayerPrefs.SetInt("BaseScore", Mathf.RoundToInt(PlayerPrefs.GetInt("BaseScore") * 1.2f));
            PlayerPrefs.SetInt("ScoreIncreaseThreshold", PlayerPrefs.GetInt("ScoreIncreaseThreshold") * 2);
        }

        DisplayScore();
    }

    private void DisplayScore()
    {
        GetComponent<TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("Score");
    }
}
