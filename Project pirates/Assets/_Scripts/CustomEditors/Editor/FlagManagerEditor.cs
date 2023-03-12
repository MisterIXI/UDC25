using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlagManager))]
public class FlagManagerEditor : Editor
{
    public static string currentFlagNameEntry = "FlagName";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // text input field
        var flagName = EditorGUILayout.TextField(currentFlagNameEntry);
        currentFlagNameEntry = flagName;
        // trigger button
        if (GUILayout.Button("Toggle Flag"))
        {
            Debug.Log($"Toggle Flag: {currentFlagNameEntry}");
            FlagManager.SetFlag(currentFlagNameEntry, !FlagManager.GetFlag(currentFlagNameEntry));
        }
        if (GUILayout.Button($"Quickflag: \"DebugFlag1\""))
        {
            FlagManager.SetFlag("DebugFlag1", !FlagManager.GetFlag("DebugFlag1"));
        }
        if (GUILayout.Button($"Quickflag: \"DebugFlag2\""))
        {
            FlagManager.SetFlag("DebugFlag2", !FlagManager.GetFlag("DebugFlag2"));
        }
        if (GUILayout.Button($"Quickflag: \"DebugFlag3\""))
        {
            FlagManager.SetFlag("DebugFlag3", !FlagManager.GetFlag("DebugFlag3"));
        }

        // draw Flags HashSet
        EditorGUILayout.LabelField("Flags:");
        if (FlagManager.Instance == null)
            EditorGUILayout.LabelField("FlagManager is not initialized yet.");
        else
        {
            foreach (var flag in FlagManager.Flags)
            {
                EditorGUILayout.LabelField(flag);
            }
            if(FlagManager.Flags.Count == 0)
                EditorGUILayout.LabelField("No flags set...");
        }
        

    }
}