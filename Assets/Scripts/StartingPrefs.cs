using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPrefs : MonoBehaviour
{
    // Reset all Player Prefs
    void Start()
    {
        PlayerPrefs.SetInt("MovingBlockSet", 0);
    }
}
