using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractionTweening))]
public class InteractionTweeningEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Toggle ShowInList"))
        {
            var target = (InteractionTweening)serializedObject.targetObject;
            Debug.Log("ShowInList: " + target.ShowInList + " -> " + !target.ShowInList);
            target.ShowInList = !target.ShowInList;
        }
        if (GUILayout.Button("Print Lerp of height"))
        {
            var target = (InteractionTweening)serializedObject.targetObject;
            Debug.Log("Lerp: " + Mathf.Lerp(target.ShowHideHeight.x, target.ShowHideHeight.y, target.TweenProgress));
        }
        base.OnInspectorGUI();
        // label

        GUILayout.Label("ShowInList: " + ((InteractionTweening)serializedObject.targetObject).ShowInList);
        GUILayout.Label("IsTweening: " + ((InteractionTweening)serializedObject.targetObject).IsTweening);
        GUILayout.Label("TweenProgress: " + ((InteractionTweening)serializedObject.targetObject).TweenProgress);
    }
}