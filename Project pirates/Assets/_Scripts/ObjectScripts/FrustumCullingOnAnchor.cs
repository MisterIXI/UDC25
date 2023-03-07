using UnityEngine;

[RequireComponent(typeof(GeometryAnchor))]
public class FrustumCullingOnAnchor : FrustumCulling
{
    protected GeometryAnchor _geometryAnchor;

    protected override void Start()
    {
        base.Start();
        _geometryAnchor = GetComponent<GeometryAnchor>();
    }
    public override bool IsCurrentlyInCameraFrustum()
    {
        return CheckBoundsForCameraFrustum(_geometryAnchor.AnchorBounds, _camera);
    }
    protected override void Update()
    {
        UpdateFrustumState(_geometryAnchor.AnchorBounds, _camera);
    }
}