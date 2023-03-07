using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FrustumCulling))]
[RequireComponent(typeof(Collider))]
public class GeometryAnchor : MonoBehaviour
{
    private FrustumCulling _frustumCulling;
    public FrustumCulling FrustumCulling
    {
        get
        {
            if (_frustumCulling == null)
                _frustumCulling = GetComponent<FrustumCulling>();
            return _frustumCulling;
        }
    }
    public string AnchorName => name;
    [field: SerializeField] public Bounds AnchorBounds { get; private set; }
    [field: SerializeField] public bool DrawBoundGizmo { get; private set; }
    [HideInInspector] public AnchorList AnchorList;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player entered {AnchorName}");
        // Player is now completely on own LevelNode/Anchorlist -> tell the LevelOrchestrator
        if (other.CompareTag("Player"))
        {
            LevelOrchestrator.SetNewAnchorList(AnchorList, FrustumCulling);
        }
    }
    private void OnDrawGizmos()
    {
        if (DrawBoundGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(AnchorBounds.center + transform.position, AnchorBounds.size);
        }
    }
}