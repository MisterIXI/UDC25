using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeometryAnchor))]
public class GeometryAnchorEditor : Editor
{
    [ExecuteInEditMode]
    private void OnEnable()
    {
        // Debug.Log("GeometryAnchorEditor.OnEnable");
        SerializedObject gA_SO = new SerializedObject(target);
        SerializedProperty collider = gA_SO.FindProperty("_collider");
        if (collider.objectReferenceValue == null)
            collider.objectReferenceValue = ((GeometryAnchor)target).GetComponent<Collider>();
        gA_SO.ApplyModifiedProperties();
        // GeometryAnchor geometryAnchor = (GeometryAnchor)target;
        // SerializedProperty collider = serializedObject.FindProperty("_collider");
        // if (collider.objectReferenceValue == null)
        //     collider.objectReferenceValue = geometryAnchor.GetComponent<Collider>();
        // Debug.Log("GeometryAnchorEditor.OnEnable collider.objectReferenceValue = " + collider.objectReferenceValue);
        // Debug.Log("GetComponent<Collider>() = " + geometryAnchor.GetComponent<Collider>());
        // SerializedObject colliderSO = new SerializedObject(collider.objectReferenceValue);
        // colliderSO.mod
        // colliderSO.ApplyModifiedProperties();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GeometryAnchor geometryAnchor = (GeometryAnchor)target;
        if (!geometryAnchor.LinkAnchorBoundsWithCollider)
        {
            // show geometryAnchor.AnchorBounds field
            SerializedProperty anchorBounds = serializedObject.FindProperty("_anchorBounds");
            EditorGUILayout.PropertyField(anchorBounds);
            if (GUILayout.Button("Copy Collider Bounds to Anchor Bounds"))
            {
                Bounds colliderBounds = ((Collider)serializedObject.FindProperty("_collider").objectReferenceValue).bounds;
                colliderBounds.center -= geometryAnchor.transform.position;
                Bounds anchorBoundsBounds = anchorBounds.boundsValue;
                anchorBoundsBounds.center = colliderBounds.center;
                anchorBoundsBounds.extents = colliderBounds.extents;
                anchorBounds.boundsValue = anchorBoundsBounds;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}