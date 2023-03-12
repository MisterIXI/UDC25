using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagManager : MonoBehaviour
{
    [field: SerializeField] private HashSet<string> _flags = new HashSet<string>();
    public static HashSet<string> Flags => Instance._flags;
    public static FlagManager Instance { get; private set; }
    public static Action<string, bool> OnFlagSet;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static bool SetFlag(string flagName, bool value)
    {
        if (Flags == null)
        {
            Debug.LogError("Flags is null");
            return false;
        }
        Debug.Log($"SetFlag {flagName} to {value}");
        bool result;
        if (value)
            result = Flags.Add(flagName);
        else
            result = Flags.Remove(flagName);

        if (!result)
            Debug.LogError($"Error setting flag {flagName} to {value}");

        OnFlagSet?.Invoke(flagName, value);
        return result;
    }
    public static bool GetFlag(string flagName)
    {
        return Flags.Contains(flagName);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}