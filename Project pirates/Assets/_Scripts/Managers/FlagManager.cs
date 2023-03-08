using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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
        OnFlagSet?.Invoke(flagName, value);
        Debug.Log($"SetFlag {flagName} to {value}");
        if (value)
            if (Flags.Add(flagName))
                return true;
            else
            {
                Debug.LogError($"Flag {flagName} already set");
                return false;
            }
        else
            if (Flags.Remove(flagName))
            return true;
        else
        {
            Debug.LogError($"Tried to remove non existing flag: {flagName}");
            return false;
        }
    }
    public static bool GetFlag(string flagName)
    {
        return Flags.Contains(flagName);
    }
}