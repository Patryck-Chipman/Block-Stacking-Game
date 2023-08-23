using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private BoardController _controller;
    [SerializeField] private FuelBar _fuelBar;

    private GameObject[][] _tileObjects;
    private IEnumerator _coroutine;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopGame()
    {
        _tileObjects = _controller.tileObjects;
        //Time.timeScale = 0;
        _fuelBar.canEmpty = false;
        _coroutine = MakeTilesGray();
        StartCoroutine(_coroutine);
    }

    private IEnumerator MakeTilesGray()
    {
        Debug.Log("Coroutine started");
        foreach (var row in _tileObjects.Reverse())
        {
            //if (!row.Contains(gameObject)) continue;

            foreach (var tile in row)
            {
                if (tile == null) continue;

                FadeTile(tile);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void FadeTile(GameObject tile)
    {
        tile.GetComponent<ChangeColor>().ChangeToGray();
    }
}
