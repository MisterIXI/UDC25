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
    [field: SerializeField] private Collider _collider;
    // private Collider Collider => _collider ?? (_collider = GetComponent<Collider>());
    public Bounds LocalAnchorBounds
    {
        get
        {
            Bounds bounds = _anchorBounds;
            if (LinkAnchorBoundsWithCollider)
            {
                bounds = _collider.bounds;
                bounds.center -= transform.position;
            }
            return bounds;
        }
    }
    public Bounds WorldSpaceAnchorBounds
    {
        get
        {
            Bounds bounds = _collider.bounds;
            if (!LinkAnchorBoundsWithCollider)
            {
                bounds = _anchorBounds;
                bounds.center += transform.position;
            }
            return bounds;
        }
    }
    [HideInInspector][field: SerializeField] private Bounds _anchorBounds;
    [field: SerializeField] public bool DrawBoundGizmo { get; private set; }
    [field: SerializeField] public bool LinkAnchorBoundsWithCollider { get; private set; } = true;
    [HideInInspector] public AnchorList AnchorList;

    private void Start()
    {
        // _collider = GetComponent<Collider>();
        // if (_collider == null)
        // {
        //     Debug.LogError("No Collider found on GeometryAnchor! Disabling GeometryAnchor...");
        //     gameObject.SetActive(false);
        // }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Player entered {AnchorName}");
        // Player is now completely on own LevelNode/Anchorlist -> tell the LevelOrchestrator
        if (other.CompareTag("Player"))
        {
            LevelOrchestrator.SetNewAnchorList(AnchorList, FrustumCulling);
        }
    }
    [ContextMenu("DebugPrintColliderBounds")]
    private void DebugPrintColliderBounds()
    {
        Debug.Log($"Collider Bounds: {WorldSpaceAnchorBounds}");

    }
    private void OnDrawGizmos()
    {
        if (DrawBoundGizmo)
        {
            if (!FrustumCulling.IsCurrentlyVisible)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawWireCube(WorldSpaceAnchorBounds.center, WorldSpaceAnchorBounds.size);
        }
    }
}