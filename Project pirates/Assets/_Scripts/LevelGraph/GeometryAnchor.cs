using System.Collections.Generic;
using UnityEngine;
public class GeometryAnchor : MonoBehaviour
{
    public string AnchorName => name;
    [field: SerializeField] public Bounds AnchorBounds { get; private set; }
    [field: SerializeField] public bool DrawBoundGizmo { get; private set; }

    private void OnDrawGizmos()
    {
        if (DrawBoundGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(AnchorBounds.center + transform.position, AnchorBounds.size);
        }
    }
}