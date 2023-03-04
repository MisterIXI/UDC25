using System;
using UnityEngine;
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettingsDefaults;
    public static PlayerSettings PlayerSettings { get; private set; }
    public static SettingsManager Instance { get; private set; }

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
    }







}