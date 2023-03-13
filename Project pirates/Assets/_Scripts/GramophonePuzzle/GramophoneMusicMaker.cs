using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GramophoneMusicMaker : MonoBehaviour
{
    private PlayerSettings _playerSettings;
    private AudioSource _audioSource;
    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.volume = _playerSettings.TotalMusicVolume;
        SettingsManager.OnMasterVolumeChanged += OnMusicVolumeChanged;
        SettingsManager.OnMusicVolumeChanged += OnMusicVolumeChanged;
    }
    private void OnMusicVolumeChanged(float newVolume)
    {
        _audioSource.volume = _playerSettings.TotalMusicVolume;
    }

    private void OnDestroy()
    {
        SettingsManager.OnMasterVolumeChanged -= OnMusicVolumeChanged;
        SettingsManager.OnMusicVolumeChanged -= OnMusicVolumeChanged;
    }
}