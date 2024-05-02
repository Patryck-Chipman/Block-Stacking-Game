using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatisticsButton : MonoBehaviour
{
    /// <summary>
    /// Method <c>Reset</c> Loads the statisctics level
    /// </summary>
    public void Reset()
    {
        SceneManager.LoadScene("Statistics");
    }
}
