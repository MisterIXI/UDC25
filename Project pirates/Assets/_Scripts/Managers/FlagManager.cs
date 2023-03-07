using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class FlagManager : MonoBehaviour
{
    public static HashSet<string> Flags = new HashSet<string>();
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
        OnFlagSet?.Invoke(flagName, value);
        if (value)
            return Flags.Add(flagName);
        else
            return Flags.Remove(flagName);
    }
    public static bool GetFlag(string flagName)
    {
        return Flags.Contains(flagName);
    }
}