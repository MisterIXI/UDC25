using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class FrustumCulling : MonoBehaviour
{
    public bool IsCurrentlyVisible { get; protected set; }
    public event Action OnEnterCameraFrustum;
    public event Action OnExitCameraFrustum;
    [field: SerializeField] protected bool _useCenterForOcclusion = true;
    [field: SerializeField] protected bool _useCornersForOcclusion = true;
    [field: SerializeField] protected bool _extraPointsInMiddle = false;
    [field: SerializeField] protected bool _extraPointsOnEdges = false;
    public event Action<FrustumCulling, bool> OnCameraFrustumStatusChangedWithSelf;
    protected Camera _camera;
    private const int CORNERBUFFERMAXSIZE = 1 + 8 + 8 + 12;
    public int CornerBufferSize => (_useCenterForOcclusion ? 1 : 0) + (_useCornersForOcclusion ? 8 : 0) + (_extraPointsInMiddle ? 8 : 0) + (_extraPointsOnEdges ? 12 : 0);
    private Vector3[] _cornerBuffer = new Vector3[CORNERBUFFERMAXSIZE];
    private void Awake()
    {
        _camera = Camera.main;
    }
    protected virtual void Start()
    {
        OnEnterCameraFrustum += () => OnCameraFrustumStatusChangedWithSelf?.Invoke(this, true);
        OnExitCameraFrustum += () => OnCameraFrustumStatusChangedWithSelf?.Invoke(this, false);

    }

    protected virtual void Update()
    {

    }

    protected virtual void UpdateFrustumState(Bounds bounds, Camera camera)
    {
        bool frustumState = CheckBoundsForCameraFrustum(bounds, camera);
        if (frustumState != IsCurrentlyVisible)
        {
            IsCurrentlyVisible = frustumState;
            if (IsCurrentlyVisible)
                OnEnterCameraFrustum?.Invoke();
            else
                OnExitCameraFrustum?.Invoke();
        }
    }
    abstract public bool IsCurrentlyInCameraFrustum();
    protected virtual Vector3[] GetAllTargetPoints(Bounds bounds)
    {
        int iterator = 0;
        Vector3 extents = bounds.extents;
        Vector3 center = bounds.center;
        if (_useCenterForOcclusion)
            _cornerBuffer[iterator++] = center;
        if (_useCornersForOcclusion)
        {
            // each corner of the bounding box
            for (int i = 0; i < 8; i++)
            {
                Vector3 corner = bounds.center + new Vector3((i & 1) == 0 ? extents.x : -extents.x,
                    (i & 2) == 0 ? extents.y : -extents.y,
                    (i & 4) == 0 ? extents.z : -extents.z);
                _cornerBuffer[iterator++] = corner;
            }
        }
        // each corner of the bounding box, but inset by half
        if (_extraPointsInMiddle)
        {
            Vector3 boundExtentsHalf = extents / 2;
            for (int i = 0; i < 8; i++)
            {
                Vector3 corner = bounds.center + new Vector3((i & 1) == 0 ? boundExtentsHalf.x : -boundExtentsHalf.x,
                    (i & 2) == 0 ? boundExtentsHalf.y : -boundExtentsHalf.y,
                    (i & 4) == 0 ? boundExtentsHalf.z : -boundExtentsHalf.z);
                _cornerBuffer[iterator++] = corner;
            }
        }
        // middle of each edge of the bounding box, points are moved clockwise
        if (_extraPointsOnEdges)
        {
            _cornerBuffer[iterator++] = center + new Vector3(0, -extents.y, -extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(0, -extents.y, extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(extents.x, 0, -extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(extents.x, 0, extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(0, extents.y, -extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(0, extents.y, extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(-extents.x, 0, -extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(-extents.x, 0, extents.z);
            _cornerBuffer[iterator++] = center + new Vector3(-extents.x, -extents.y, 0);
            _cornerBuffer[iterator++] = center + new Vector3(extents.x, -extents.y, 0);
            _cornerBuffer[iterator++] = center + new Vector3(-extents.x, extents.y, 0);
            _cornerBuffer[iterator++] = center + new Vector3(extents.x, extents.y, 0);
        }
        return _cornerBuffer;
    }
    protected virtual bool CheckBoundsForCameraFrustum(Bounds bounds, Camera camera)
    {
        // check center of bounding box
        if (IsPosInCameraFrustum(bounds.center, camera))
            return true;
        // for each corner of the object's bounding box
        Vector3 boundExtents = bounds.extents;
        Vector3[] corners = GetAllTargetPoints(bounds);
        for (int i = 0; i < CornerBufferSize; i++)
        {
            // if the corner is inside the camera's frustum, the object is visible
            if (IsPosInCameraFrustum(corners[i], camera))
                return true;
        }
        return false;
    }

    protected bool IsPosInCameraFrustum(Vector3 position, Camera camera)
    {
        Vector3 frustumPos = camera.WorldToViewportPoint(position);
        return frustumPos.x > 0 && frustumPos.x < 1 && frustumPos.y > 0 && frustumPos.y < 1 && frustumPos.z > 0;
    }
}
