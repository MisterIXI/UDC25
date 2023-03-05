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
        SettingsManager.PlayerSettings.MasterVolume = value;
    }

    private void OnSliderMusic(float value)
    {
        // set music volume
        SettingsManager.PlayerSettings.MusicVolume = value;
    }

    private void OnSliderSFX(float value)
    {
        // set sfx volume
        SettingsManager.PlayerSettings.SfxVolume = value;
    }

    private void OnToggleYInvert(bool value)
    {
        // set y invert
        SettingsManager.PlayerSettings.InvertYAxis = value;
    }

    private void OnSliderMouseSense(float value)
    {
        // set mouse sense
        SettingsManager.PlayerSettings.MouseLookSensitivity = value;
    }

    private void OnSliderGamepadSense(float value)
    {
        // set gamepad sense
        SettingsManager.PlayerSettings.GamepadLookSensitivity = value;
    }
}

