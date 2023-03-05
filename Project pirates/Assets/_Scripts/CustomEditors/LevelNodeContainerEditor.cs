using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelNodeContainer))]
public class LevelNodeContainerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var container = (LevelNodeContainer)target;
        var dict = container.levelNodeLinkDataDictionary;
        // show dictionary contents
        foreach (var key in dict.Keys) {
            var list = dict[key];
            foreach (var item in list) {
                EditorGUILayout.LabelField(item.BaseNodeGUID);
                EditorGUILayout.LabelField(item.BasePortName);
                EditorGUILayout.LabelField(item.TargetNodeGUID);
                EditorGUILayout.LabelField(item.TargetPortName);
            }
        }

        
    }
}