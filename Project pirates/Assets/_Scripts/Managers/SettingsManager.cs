using System;
using UnityEngine;
public class SettingsManager : MonoBehaviour
{
    [field: SerializeField] private GramophoneSettings _gramophoneSettings;
    public static GramophoneSettings GramophoneSettings => Instance._gramophoneSettings;
    [SerializeField] private PlayerSettings _playerSettingsDefaults;
    public static PlayerSettings PlayerSettings { get; private set; }
    public static SettingsManager Instance { get; private set; }
    public static event Action<float> OnMasterVolumeChanged;
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;
    public static event Action<float> OnMouseLookSensitivityChanged;
    public static event Action<float> OnGamepadLookSensitivityChanged;
    public static event Action<bool> OnInvertYAxisChanged;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (_playerSettingsDefaults.CloneScriptableObjectOnStart)
        {
            PlayerSettings = _playerSettingsDefaults.Clone();
        }
        else
        {
            PlayerSettings = _playerSettingsDefaults;
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void SetMasterVolume(float value)
    {
        PlayerSettings.MasterVolume = value;
        OnMasterVolumeChanged?.Invoke(value);
    }

    public static void SetMusicVolume(float value)
    {
        PlayerSettings.MusicVolume = value;
        OnMusicVolumeChanged?.Invoke(value);
    }

    public static void SetSFXVolume(float value)
    {
        PlayerSettings.SfxVolume = value;
        OnSFXVolumeChanged?.Invoke(value);
    }

    public static void SetMouseLookSensitivity(float value)
    {
        PlayerSettings.MouseLookSensitivity = value;
        OnMouseLookSensitivityChanged?.Invoke(value);
    }

    public static void SetGamepadLookSensitivity(float value)
    {
        PlayerSettings.GamepadLookSensitivity = value;
        OnGamepadLookSensitivityChanged?.Invoke(value);
    }

    public static void SetInvertYAxis(bool value)
    {
        PlayerSettings.InvertYAxis = value;
        OnInvertYAxisChanged?.Invoke(value);
    }





}