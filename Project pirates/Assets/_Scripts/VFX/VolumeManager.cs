using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance { get; private set; }
    private Volume _volume;
    private Vignette _vignette;
    private Bloom _bloom;
    private DepthOfField _depthOfField;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _volume = GetComponent<Volume>();
        if (!_volume.profile.TryGet(out _vignette))
        {
            Debug.LogError("Vignette not found");
        }
        if (!_volume.profile.TryGet(out _bloom))
        {
            Debug.LogError("Bloom not found");
        }
        if (!_volume.profile.TryGet(out _depthOfField))
        {
            Debug.LogError("Motion Blur not found");
        }
    }

    public static void SetVignetteStatus(bool status)
    {
        if (Instance._vignette != null)
        {
            Instance._vignette.active = status;
        }
    }

    public static void SetBloomStatus(bool status)
    {
        if (Instance._bloom != null)
        {
            Instance._bloom.active = status;
        }
    }

    public static void SetDepthOfFieldStatus(bool status)
    {
        if (Instance._depthOfField != null)
        {
            Instance._depthOfField.active = status;
        }
    }
}
