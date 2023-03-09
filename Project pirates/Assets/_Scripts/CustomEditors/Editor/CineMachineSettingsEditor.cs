using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CineMachineSettings))]
public class CineMachineSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // SerializedObject sO = new SerializedObject(target);
        // EditorGUILayout.LabelField("Variables:");
        // EditorGUILayout.LabelField($"horizontalSpeed: {sO.FindProperty("horizontalSpeed").floatValue}");
        // EditorGUILayout.LabelField($"verticalSpeed: {sO.FindProperty("verticalSpeed").floatValue}");
        // EditorGUILayout.LabelField($"clampAngle: {sO.FindProperty("clampAngle").floatValue}");
        // EditorGUILayout.LabelField($"IsKeyboardAndMouseActive: {sO.FindProperty("IsKeyboardAndMouseActive").boolValue}");
    }
}