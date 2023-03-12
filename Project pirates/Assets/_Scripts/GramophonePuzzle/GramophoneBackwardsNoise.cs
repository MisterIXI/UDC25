using System;
using System.Collections;
using UnityEngine;
public class GramophoneBackwardsNoise : MonoBehaviour
{
    private GramophoneSettings _gramophoneSettings;
    public static Action<bool> OnGramophoneBackwardsNoiseTrigger;
    private int _nodeChangesSinceLastTrigger;
    private float _timeSinceLastTrigger;
    private bool _isTriggered;
    private void Start()
    {
        _gramophoneSettings = SettingsManager.GramophoneSettings;
        LevelOrchestrator.OnCurrentNodeChanged += OnCurrentNodeChanged;
    }

    private void OnCurrentNodeChanged()
    {
        if (_isTriggered)
            return;
        _nodeChangesSinceLastTrigger++;
        if (_nodeChangesSinceLastTrigger >= _gramophoneSettings.GramoBackwardsMinimumNodeChanges)
        {
            if (Time.time - _timeSinceLastTrigger >= _gramophoneSettings.GramoBackwardsNoiseTriggerDuration)
            {
                TriggerGramophoneBackwardsNoise();
            }
        }
    }
    private void TriggerGramophoneBackwardsNoise()
    {
        OnGramophoneBackwardsNoiseTrigger?.Invoke(true);
        _nodeChangesSinceLastTrigger = 0;
        _timeSinceLastTrigger = Time.time;
        StartCoroutine(DelayedUntrigger());
        _isTriggered = true;
    }
    private IEnumerator DelayedUntrigger()
    {
        yield return new WaitForSeconds(_gramophoneSettings.GramoBackwardsNoiseTriggerDuration);
        UnTriggerGramophoneBackwardsNoise();
    }
    private void UnTriggerGramophoneBackwardsNoise()
    {
        OnGramophoneBackwardsNoiseTrigger?.Invoke(false);
        _isTriggered = false;
    }

    private void OnDestroy()
    {
        LevelOrchestrator.OnCurrentNodeChanged -= OnCurrentNodeChanged;
    }
}