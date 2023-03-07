using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FrustumCullingOnMesh : FrustumCulling
{
    private MeshRenderer _meshRenderer;

    protected override void Start()
    {
        base.Start();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public override bool IsCurrentlyInCameraFrustum()
    {
        return CheckBoundsForCameraFrustum(_meshRenderer.bounds, _camera);
    }
    protected override void Update()
    {
        base.Update();
        UpdateFrustumState(_meshRenderer.bounds, _camera);
    }
}