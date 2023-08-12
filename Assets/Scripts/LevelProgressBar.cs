using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelProgressBar : MonoBehaviour
{
    // TODO:

    public Image mask;

    private IEnumerator coroutine;
    private int _min = 0;
    private int _max = 0;

    private void Start()
    {
        _min = 0;
        _max = 300;
    }

    /// <summary>
    /// Method <c>CalculateFillPercent</c> begins the displaying progress in the progress bar
    /// </summary>
    /// <param name="current"></param> is the current score of the player
    public void CalculateFillPercent(int current)
    {
        if (coroutine != null) StopCoroutine(coroutine);

        float offset = current - _min;
        float _maxOffset = _max - _min;
        float fillAmount = (float)offset / (float)_maxOffset;

        if (fillAmount > 1) fillAmount = 1;

        coroutine = Fill(fillAmount);
        StartCoroutine(Fill(fillAmount));
    }

    // Bring the progress bar back to 0
    private void ResetBar()
    {
        _min = _max;
        _max = PlayerPrefs.GetInt("ScoreIncreaseThreshold");
        CalculateFillPercent(PlayerPrefs.GetInt("Score"));
        mask.fillAmount = 0;
    }

    // Gradually fill the progress bar
    IEnumerator Fill(float fillAmount)
    {
        if (fillAmount < mask.fillAmount) yield return null;

        while (mask.fillAmount < fillAmount)
        {
            mask.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }

        if (fillAmount >= 1) ResetBar();
    }
}
