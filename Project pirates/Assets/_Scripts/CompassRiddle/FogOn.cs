using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOn : MonoBehaviour
{
    private PlayerSettings _playerSettings;
    public static bool KillFog = false;

    void Start()
    {
        if (KillFog)
            return;
        _playerSettings = SettingsManager.PlayerSettings;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0f;
    }


    void FixedUpdate()
    {
        if (KillFog)
            return;
        if (RenderSettings.fogDensity != _playerSettings.FogDensity)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, _playerSettings.FogDensity, Time.deltaTime);
        }
        if (VolumeManager.Sun.intensity < _playerSettings.SunIntensity)
        {
            VolumeManager.SetSunIntensity(Mathf.Lerp(VolumeManager.Sun.intensity, _playerSettings.SunIntensity, Time.deltaTime));
        }
    }
}
