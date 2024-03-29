using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image _mask;
    [SerializeField]
    private LevelLabel _levelLabel;

    private IEnumerator coroutine;
    private int _min = 0;
    private int _max = 0;

    private void Start()
    {
        _min = 0;
        _max = 1000;
        //_percentLabel.DisplayPercentage(0);
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
        StartCoroutine(coroutine);
    }

    // Bring the progress bar back to 0
    private void ResetBar()
    {
        _levelLabel.IncreaseLevel();
        _min = _max;
        _max = PlayerPrefs.GetInt("ScoreIncreaseThreshold");
        CalculateFillPercent(PlayerPrefs.GetInt("Score"));
        GetComponent<ClassicProgressBar>().m_FillAmount = 0;
        //_mask.fillAmount = 0;
    }

    // Gradually fill the progress bar
    IEnumerator Fill(float fillAmount)
    {
        if (fillAmount < /*_mask.fillAmount*/GetComponent<ClassicProgressBar>().m_FillAmount) yield return null;

        while (/*_mask.fillAmount*/GetComponent<ClassicProgressBar>().m_FillAmount < fillAmount)
        {
            GetComponent<ClassicProgressBar>().m_FillAmount += 0.01f;
            //_mask.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }

        if (fillAmount >= 1) ResetBar();
    }
}
