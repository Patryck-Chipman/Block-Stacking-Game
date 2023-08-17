using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class Text : MonoBehaviour
{
    [SerializeField] float quotient;

    // Start is called before the first frame update
    void Start()
    {
        float textSize = Screen.currentResolution.width / quotient;
        GetComponent<TextMeshProUGUI>().fontSize = textSize;
    }
}
