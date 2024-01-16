using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    /// <summary>
    /// Method <c>Reset</c> Loads the main menu
    /// </summary>
    public void Reset()
    {
        SceneManager.LoadScene("Home");
    }
}
