using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FuelBar : MonoBehaviour
{
    [SerializeField]
    private Image _mask;

    private const float MAX_FUEL = 1200; // Twenty seconds worth of fuel
    private float _fuel;

    // Start is called before the first frame update
    void Start()
    {
        _fuel = MAX_FUEL;
        _mask.fillAmount = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_fuel > MAX_FUEL) _fuel = MAX_FUEL;
        if (_fuel < 0) _fuel = 0;

        _fuel--;
        _mask.fillAmount = _fuel / MAX_FUEL;
    }

    /// <summary>
    /// Method <c>IncreaseFuel</c> adds fuel to make the game the game last longer
    /// </summary>
    /// <param name="amount"></param> is the amount of fuel (frames) to be added
    public void IncreaseFuel(int amount)
    {
        _fuel += amount;
    }
}
