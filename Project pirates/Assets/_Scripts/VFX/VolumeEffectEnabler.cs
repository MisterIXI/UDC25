using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeEffectEnabler : MonoBehaviour
{
    [field: SerializeField] public bool ToggleVignette { get; private set; } = true;
    [field: SerializeField] public bool ToggleBloom { get; private set; } = true;
    [field: SerializeField] public bool ToggleDepthOfField { get; private set; } = true;

    private void OnEnable()
    {
        if (ToggleVignette) VolumeManager.SetVignetteStatus(true);
        if (ToggleBloom) VolumeManager.SetBloomStatus(true);
        if (ToggleDepthOfField) VolumeManager.SetDepthOfFieldStatus(true);
    }

    private void OnDisable()
    {
        if (ToggleVignette) VolumeManager.SetVignetteStatus(false);
        if (ToggleBloom) VolumeManager.SetBloomStatus(false);
        if (ToggleDepthOfField) VolumeManager.SetDepthOfFieldStatus(false);
    }
}
