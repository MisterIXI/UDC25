using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnchorList))]
public class AnchorListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate GUID"))
        {
            ((AnchorList)target).GUID = System.Guid.NewGuid().ToString();
        }
        base.OnInspectorGUI();
    }
}