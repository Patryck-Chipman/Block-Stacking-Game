using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FuelBar : MonoBehaviour
{
    [SerializeField]
    private Image _mask;
    [SerializeField]
    private GameOver _gameOver;

    /// <summary>
    /// Field <c>fuel</c> is the float value of fuel remaining
    /// </summary>
    public float fuel { get; private set; }
    private  const float START_FUEL = 600; // Roughly ten seconds worth of fuel
    private float _maxFuel = START_FUEL;

    // Start is called before the first frame update
    void Start()
    {
        fuel = _maxFuel;
        GetComponent<CircularProgressBar>().m_FillAmount = 1;
        //_mask.fillAmount = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _maxFuel = START_FUEL - (PlayerPrefs.GetInt("CurrentLevel") - 1) * 20;
        if (_maxFuel <= 300) _maxFuel = 300;

        if (fuel == 0 && PlayerPrefs.GetInt("Game Over") == 0) _gameOver.StopGame();
        if (fuel > _maxFuel) fuel = _maxFuel;
        if (fuel < 0) fuel = 0;

        if (PlayerPrefs.GetInt("Game Over") == 0) fuel--;
        //_mask.fillAmount = fuel / _maxFuel;
        GetComponent<CircularProgressBar>().m_FillAmount = fuel / _maxFuel;
    }

    /// <summary>
    /// Method <c>IncreaseFuel</c> adds fuel to make the game the game last longer
    /// </summary>
    /// <param name="amount"></param> is the amount of fuel (frames) to be added
    public void IncreaseFuel(int amount)
    {
        fuel += amount;
    }
}
