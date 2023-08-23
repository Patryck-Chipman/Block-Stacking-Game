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
    /// Field <c>canEmpty</c> is wheter or not to drain fuel
    /// </summary>
    [HideInInspector] public bool canEmpty;
    /// <summary>
    /// Field <c>fuel</c> is the float value of fuel remaining
    /// </summary>
    public float fuel { get; private set; }

    private const float MAX_FUEL = 600; // Roughly ten seconds worth of fuel

    // Start is called before the first frame update
    void Start()
    {
        canEmpty = true;
        fuel = MAX_FUEL;
        _mask.fillAmount = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fuel == 0 && canEmpty) _gameOver.StopGame();
        if (fuel > MAX_FUEL) fuel = MAX_FUEL;
        if (fuel < 0) fuel = 0;

        if (canEmpty) fuel--;
        _mask.fillAmount = fuel / MAX_FUEL;
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
