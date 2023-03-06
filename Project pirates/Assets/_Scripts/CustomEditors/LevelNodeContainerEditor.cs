using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelNodeContainer))]
public class LevelNodeContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open LevelGraph Window"))
        {
            LevelGraph.ShowWindow(target as LevelNodeContainer);
        }
        base.OnInspectorGUI();
    }
}