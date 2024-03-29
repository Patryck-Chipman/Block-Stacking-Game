using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private BoardController _controller;
    [SerializeField] private GameObject _gameOverUI;

    private GameObject[][] _tileObjects;
    private IEnumerator _coroutine;

    private void Start()
    {
        PlayerPrefs.SetInt("Game Over", 0);

        _gameOverUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Method <c>StopGame</c> pauses the game and displays the game over menu
    /// </summary>
    public void StopGame()
    {
        PlayerPrefs.SetInt("Total Games", PlayerPrefs.GetInt("Total Games") + 1);
        PlayerPrefs.SetInt("Game Over", 1);
        _tileObjects = _controller.tileObjects;
        _coroutine = MakeTilesGray();
        StartCoroutine(_coroutine);
        _gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator MakeTilesGray()
    {
        Debug.Log("Coroutine started");
        foreach (var row in _tileObjects.Reverse())
        {
            foreach (var tile in row)
            {
                if (tile == null) continue;

                FadeTile(tile);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FadeTile(GameObject tile)
    {
        tile.GetComponent<ChangeColor>().ChangeToGray();
    }
}
