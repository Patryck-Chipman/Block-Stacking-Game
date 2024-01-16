using TMPro;
using UnityEngine;

public class LevelLabel : MonoBehaviour
{
    private int _level;

    // Start is called before the first frame update
    void Start()
    {
        _level = 1;
        PlayerPrefs.SetInt("CurrentLevel", _level);
        DisplayLevel();
    }

    /// <summary>
    /// Method <c>IncreaseLevel</c> increases the current level by 1 and displays it
    /// </summary>
    public void IncreaseLevel()
    {
        _level++;
        PlayerPrefs.SetInt("CurrentLevel", _level);
        DisplayLevel();
        if (_level == 10) PlayerPrefs.SetInt("Times Level 10 Reached", PlayerPrefs.GetInt("Times Level 10 Reached") + 1);
        if (_level == 25) PlayerPrefs.SetInt("Times Level 25 Reached", PlayerPrefs.GetInt("Times Level 25 Reached") + 1);
        if (_level == 50) PlayerPrefs.SetInt("Times Level 50 Reached", PlayerPrefs.GetInt("Times Level 50 Reached") + 1);
        if (_level == 100) PlayerPrefs.SetInt("Times Level 100 Reached", PlayerPrefs.GetInt("Times Level 100 Reached") + 1);
        if (_level > PlayerPrefs.GetInt("Highest Level")) PlayerPrefs.SetInt("Highest Level", _level);
    }

    // Change the level text to the new level
    private void DisplayLevel()
    {
        GetComponent<TextMeshProUGUI>().text = _level.ToString();
    }
}
