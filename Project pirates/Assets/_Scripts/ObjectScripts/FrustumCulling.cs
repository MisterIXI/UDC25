using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class FrustumCulling : MonoBehaviour
{
    public bool IsInCameraFrustum { get; private set; }
    public event Action OnEnterCameraFrustum;
    public event Action<FrustumCulling> OnEnterCameraFrustumWithSelf;
    public event Action OnExitCameraFrustum;
    public event Action<FrustumCulling> OnExitCameraFrustumWithSelf;
    protected Camera _camera;
    protected virtual void Start()
    {
        _camera = Camera.main;
        OnEnterCameraFrustum += () => OnEnterCameraFrustumWithSelf?.Invoke(this);
        OnExitCameraFrustum += () => OnExitCameraFrustumWithSelf?.Invoke(this);
    }

    protected virtual void Update()
    {

    }

    protected virtual void UpdateFrustumState(Bounds bounds, Camera camera)
    {
        bool frustumState = CheckBoundsForCameraFrustum(bounds, camera);
        if (frustumState != IsInCameraFrustum)
        {
            IsInCameraFrustum = frustumState;
            if (IsInCameraFrustum)
                OnEnterCameraFrustum?.Invoke();
            else
                OnExitCameraFrustum?.Invoke();
        }
    }
    abstract public bool IsCurrentlyInCameraFrustum();
    protected virtual bool CheckBoundsForCameraFrustum(Bounds bounds, Camera camera)
    {
        // check center of bounding box
        if (IsPosInCameraFrustum(bounds.center, camera))
            return true;
        // for each corner of the object's bounding box
        Vector3 boundExtents = bounds.extents;
        for (int i = 0; i < 8; i++)
        {
            Vector3 corner = transform.position + new Vector3((i & 1) == 0 ? boundExtents.x : -boundExtents.x,
                (i & 2) == 0 ? boundExtents.y : -boundExtents.y,
                (i & 4) == 0 ? boundExtents.z : -boundExtents.z);
            // if the corner is inside the camera's frustum, the object is visible
            if (IsPosInCameraFrustum(corner, camera))
                return true;
        }
        return false;
    }

    private bool IsPosInCameraFrustum(Vector3 position, Camera camera)
    {
        Vector3 frustumPos = camera.WorldToViewportPoint(position);
        return frustumPos.x > 0 && frustumPos.x < 1 && frustumPos.y > 0 && frustumPos.y < 1 && frustumPos.z > 0;
    }
}
