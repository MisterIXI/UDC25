using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeContainer))]
public class NodeContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open LevelGraph Window"))
        {
            var levelGraphWindow = LevelGraph.ShowWindow(target as NodeContainer);
            
        }
        base.OnInspectorGUI();
    }
}