using UnityEngine;
using UnityEngine.UI;
public class MenuSettings : MenuBase
{
    [field: SerializeField] public Button _buttonBack { get; private set; }
    [field: SerializeField] public Slider _sliderMasterVolume { get; private set; }
    [field: SerializeField] public Slider _sliderMusicVolume { get; private set; }
    [field: SerializeField] public Slider _sliderSFXVolume { get; private set; }
    [field: SerializeField] public Toggle _toggleYInvert { get; private set; }
    [field: SerializeField] public Slider _sliderMouseSense { get; private set; }
    [field: SerializeField] public Slider _sliderGamepadSense { get; private set; }

    override public void Initialize()
    {
        base.Initialize();
        AddListeners();
        SetValuesFromSettings();
        SubscribeToChanges();
    }
    private void SetValuesFromSettings()
    {
        _sliderMasterVolume.value = SettingsManager.PlayerSettings.MasterVolume;
        _sliderMusicVolume.value = SettingsManager.PlayerSettings.MusicVolume;
        _sliderSFXVolume.value = SettingsManager.PlayerSettings.SfxVolume;
        _toggleYInvert.isOn = SettingsManager.PlayerSettings.InvertYAxis;
        _sliderMouseSense.value = SettingsManager.PlayerSettings.MouseLookSensitivity;
        _sliderGamepadSense.value = SettingsManager.PlayerSettings.GamepadLookSensitivity;
        _toggleYInvert.isOn = SettingsManager.PlayerSettings.InvertYAxis;
    }
    private void AddListeners()
    {
        _buttonBack.onClick.AddListener(OnBack);
        _sliderMasterVolume.onValueChanged.AddListener(OnSliderMaster);
        _sliderMusicVolume.onValueChanged.AddListener(OnSliderMusic);
        _sliderSFXVolume.onValueChanged.AddListener(OnSliderSFX);
        _toggleYInvert.onValueChanged.AddListener(OnToggleYInvert);
        _sliderMouseSense.onValueChanged.AddListener(OnSliderMouseSense);
        _sliderGamepadSense.onValueChanged.AddListener(OnSliderGamepadSense);
    }

    private void SubscribeToChanges()
    {
        SettingsManager.OnMasterVolumeChanged += OnMasterVolumeChanged;
        SettingsManager.OnMusicVolumeChanged += OnMusicVolumeChanged;
        SettingsManager.OnSFXVolumeChanged += OnSFXVolumeChanged;
        SettingsManager.OnMouseLookSensitivityChanged += OnMouseLookSensitivityChanged;
        SettingsManager.OnGamepadLookSensitivityChanged += OnGamepadLookSensitivityChanged;
        SettingsManager.OnInvertYAxisChanged += OnInvertYAxisChanged;
    }
    public override void SetSelection()
    {
        _sliderMasterVolume.Select();
    }
    private void OnBack()
    {
        MenuManager.ShowPreviousMenu();
    }

    private void OnSliderMaster(float value)
    {
        // set master volume
        SettingsManager.SetMasterVolume(value);
    }

    private void OnSliderMusic(float value)
    {
        // set music volume
        SettingsManager.SetMusicVolume(value);
    }

    private void OnSliderSFX(float value)
    {
        // set sfx volume
        SettingsManager.SetSFXVolume(value);
    }

    private void OnToggleYInvert(bool value)
    {
        // set y invert
        SettingsManager.SetInvertYAxis(value);
    }

    private void OnSliderMouseSense(float value)
    {
        // set mouse sense
        SettingsManager.SetMouseLookSensitivity(value);
    }

    private void OnSliderGamepadSense(float value)
    {
        // set gamepad sense
        SettingsManager.SetGamepadLookSensitivity(value);
    }

    private void OnMasterVolumeChanged(float value)
    {
        _sliderMasterVolume.SetValueWithoutNotify(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        _sliderMusicVolume.SetValueWithoutNotify(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        _sliderSFXVolume.SetValueWithoutNotify(value);
    }

    private void OnMouseLookSensitivityChanged(float value)
    {
        _sliderMouseSense.SetValueWithoutNotify(value);
    }

    private void OnGamepadLookSensitivityChanged(float value)
    {
        _sliderGamepadSense.SetValueWithoutNotify(value);
    }

    private void OnInvertYAxisChanged(bool value)
    {
        _toggleYInvert.SetIsOnWithoutNotify(value);
    }


}

