using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class FrustumCulling : MonoBehaviour
{
    public bool IsInCameraFrustum { get; private set; }
    public event Action OnEnterCameraFrustum;
    public event Action OnExitCameraFrustum;
    private MeshRenderer _meshRenderer;
    private Camera _camera;
    void Start()
    {
        _camera = Camera.main;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        bool frustumState = CheckForCameraFrustum();
        if (frustumState != IsInCameraFrustum)
        {
            IsInCameraFrustum = frustumState;
            if (IsInCameraFrustum)
                OnEnterCameraFrustum?.Invoke();
            else
                OnExitCameraFrustum?.Invoke();
        }
    }

    private bool CheckForCameraFrustum()
    {
        Vector3 bounds = _meshRenderer.bounds.extents;
        // for each corner of the object's bounding box
        for (int i = 0; i < 8; i++)
        {
            Vector3 corner = transform.position + new Vector3((i & 1) == 0 ? bounds.x : -bounds.x,
                (i & 2) == 0 ? bounds.y : -bounds.y,
                (i & 4) == 0 ? bounds.z : -bounds.z);
            // if the corner is inside the camera's frustum, the object is visible
            if (IsPosInCameraFrustum(corner))
                return true;
        }
        return false;
    }

    private bool IsPosInCameraFrustum(Vector3 position)
    {
        Vector3 frustumPos = _camera.WorldToViewportPoint(position);
        return frustumPos.x > 0 && frustumPos.x < 1 && frustumPos.y > 0 && frustumPos.y < 1 && frustumPos.z > 0;
    }
}
