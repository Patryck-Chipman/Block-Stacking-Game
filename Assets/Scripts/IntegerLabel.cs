using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegerLabel : MonoBehaviour
{
    public string format(int value)
    {
        return value.ToString("N0");
    }
}
