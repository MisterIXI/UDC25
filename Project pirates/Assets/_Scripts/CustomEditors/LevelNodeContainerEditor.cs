using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeContainer))]
public class LevelNodeContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open LevelGraph Window"))
        {
            LevelGraph.ShowWindow(target as NodeContainer);
        }
        base.OnInspectorGUI();
    }
}