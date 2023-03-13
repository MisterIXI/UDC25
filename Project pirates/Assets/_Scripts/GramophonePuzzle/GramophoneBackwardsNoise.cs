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
    private AudioSource _audioSource;
    private void Start()
    {
        _gramophoneSettings = SettingsManager.GramophoneSettings;
        LevelOrchestrator.OnCurrentNodeChanged += OnCurrentNodeChanged;
        _audioSource = GetComponent<AudioSource>();
        // _audioSource.clip = SoundManager.AudioClips.GrammophonMusic;
        // _audioSource.loop = true;
        // _audioSource.Play();
    }

    private void OnCurrentNodeChanged()
    {
        if (_isTriggered)
            return;
        _nodeChangesSinceLastTrigger++;
        if (UnityEngine.Random.Range(0, 100) < _gramophoneSettings.GramoBackwardsNoiseTriggerChance)
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
        // play audio in reverse
        _audioSource.pitch = -1;
        _nodeChangesSinceLastTrigger = 0;
        _timeSinceLastTrigger = Time.time;
        StartCoroutine(DelayedUntrigger());
        _isTriggered = true;
        FlagManager.SetFlag("BackwardsTrigger", true);
    }
    private IEnumerator DelayedUntrigger()
    {
        yield return new WaitForSeconds(_gramophoneSettings.GramoBackwardsNoiseTriggerDuration);
        UnTriggerGramophoneBackwardsNoise();
    }
    private void UnTriggerGramophoneBackwardsNoise()
    {
        _audioSource.pitch = 1;
        _isTriggered = false;
        _nodeChangesSinceLastTrigger = 0;
        _timeSinceLastTrigger = Time.time;
        OnGramophoneBackwardsNoiseTrigger?.Invoke(false);
        FlagManager.SetFlag("BackwardsTrigger", false);
    }

    private void OnDestroy()
    {
        LevelOrchestrator.OnCurrentNodeChanged -= OnCurrentNodeChanged;
    }
}