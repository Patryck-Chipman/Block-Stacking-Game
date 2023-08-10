using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    /// <summary>
    /// Method <c>Reset</c> resets the level inlcuding score
    /// </summary>
    public void Reset()
    {
        SceneManager.LoadScene("Level");
    }
}
