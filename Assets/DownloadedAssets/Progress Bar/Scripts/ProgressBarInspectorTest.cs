using System;
using MagicPigGames;
using UnityEngine;


public class ProgressBarInspectorTest : MonoBehaviour
{
    [SerializeField] private Character.Character _character;
    private ProgressBar _progressBar;

    void Start()
    {
        if (_progressBar == null)
            _progressBar = GetComponent<ProgressBar>();

        _character.HealthChanged += UpdateProgressBar;
    }

    private void UpdateProgressBar(float healthPercentage)
    {
        healthPercentage = Mathf.Clamp(healthPercentage, 0f, 1f);
        _progressBar.SetProgress(healthPercentage);
    }

    void OnDestroy()
    {
        _character.HealthChanged -= UpdateProgressBar;
    }
}