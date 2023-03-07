using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FrustumCullingOnAnchorWithCollider : FrustumCullingOnAnchor
{
    private Collider _collider;
    public bool IsPlayerInCollider = false;
    protected override void Start()
    {
        base.Start();
        _collider = GetComponent<Collider>();
        if (_collider.isTrigger == false)
            Debug.LogWarning("Collider is not a trigger. This will cause problems with the FrustumCullingOnAnchorWithCollider script.");
    }
    public override bool IsCurrentlyInCameraFrustum()
    {
        return IsCurrentlyVisible;
        // if (!IsPlayerInCollider)
        //     return false;
        // return CheckBoundsForCameraFrustum(_geometryAnchor.AnchorBounds, _camera);
    }
    protected override void Update()
    {
        // if (IsPlayerInCollider)
        //     UpdateFrustumState(_geometryAnchor.AnchorBounds, _camera);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsCurrentlyVisible = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsCurrentlyVisible = true;
    }
}