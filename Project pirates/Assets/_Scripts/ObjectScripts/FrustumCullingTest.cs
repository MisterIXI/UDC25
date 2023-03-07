using UnityEngine;
[RequireComponent(typeof(FrustumCulling))]
public class FrustumCullingTest : MonoBehaviour
{
    private FrustumCulling _frustumCulling;
    private MeshRenderer _meshRenderer;
    private void Awake()
    {
        _frustumCulling = GetComponent<FrustumCulling>();
        _frustumCulling.OnEnterCameraFrustum += OnEnterCameraFrustum;
        _frustumCulling.OnExitCameraFrustum += OnExitCameraFrustum;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {
        _frustumCulling.OnEnterCameraFrustum -= OnEnterCameraFrustum;
        _frustumCulling.OnExitCameraFrustum -= OnExitCameraFrustum;
    }

    private void OnEnterCameraFrustum()
    {
        Debug.Log("OnEnterCameraFrustum");
        // _meshRenderer.material.color = Color.green;
    }

    private void OnExitCameraFrustum()
    {
        Debug.Log("OnExitCameraFrustum");
        // _meshRenderer.material.color = Color.red;
    }
}