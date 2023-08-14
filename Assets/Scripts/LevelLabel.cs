using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLabel : MonoBehaviour
{
    private int _level;

    // Start is called before the first frame update
    void Start()
    {
        _level = 1;
        DisplayLevel();
    }

    /// <summary>
    /// Method <c>IncreaseLevel</c> increases the current level by 1 and displays it
    /// </summary>
    public void IncreaseLevel()
    {
        _level++;
        DisplayLevel();
    }

    private void DisplayLevel()
    {
        GetComponent<TextMeshProUGUI>().text = _level.ToString();
    }
}
