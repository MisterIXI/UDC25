using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FrustumCullingWithOcclusion : FrustumCullingOnAnchor
{
    private Collider _collider;
    [field: SerializeField] private bool _drawOcclusionRayGizmos = false;

    protected override void Start()
    {
        base.Start();
        var colliders = GetComponents<Collider>();
        if (colliders.Length > 1)
            Debug.LogWarning("More than one collider on " + gameObject.name + ". Using first collider.");
        _collider = colliders[0];
    }
    public override bool IsCurrentlyInCameraFrustum()
    {
        return CheckBoundsForCameraFrustum(_geometryAnchor.WorldSpaceAnchorBounds, _camera);
    }

    protected override void Update()
    {
        UpdateFrustumState(_geometryAnchor.WorldSpaceAnchorBounds, _camera);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsCurrentlyVisible = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            IsCurrentlyVisible = false;
    }

    protected override bool CheckBoundsForCameraFrustum(Bounds bounds, Camera camera)
    {
        Vector3 boundExtents = bounds.extents;
        Vector3[] corners = GetAllTargetPoints(bounds);
        int bufferSize = CornerBufferSize;
        for (int i = 0; i < bufferSize; i++)
        {
            if (IsPosInCameraFrustum(corners[i], camera))
                if (!IsPosOccluded(corners[i], camera))
                    return true;
        }
        return false;
    }

    private bool IsPosOccluded(Vector3 position, Camera camera)
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, position - camera.transform.position, out hit, Vector3.Distance(camera.transform.position, position)))
        {
            if (hit.collider == _collider)
                return false;
            else
                return true;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        // draw rays used for occlusion
        if (_drawOcclusionRayGizmos && _geometryAnchor != null)
        {
            Gizmos.color = Color.red;
            Bounds bounds = _geometryAnchor.WorldSpaceAnchorBounds;
            Vector3 boundExtents = bounds.extents;
            Vector3[] corners = GetAllTargetPoints(bounds);
            Camera mainCam = Camera.main;
            int bufferSize = CornerBufferSize;
            for (int i = 0; i < bufferSize; i++)
            {
                Gizmos.DrawLine(mainCam.transform.position, corners[i]);
            }
        }
    }
}