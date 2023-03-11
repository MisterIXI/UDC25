using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FrustumCulling))]
public class FrustumCullingDoor : MonoBehaviour
{    
    private FrustumCulling _frustumCulling;
    private DoorInteraction _doorInteraction;
    private MeshRenderer _meshRenderer;
    private void Awake()
    {
        _frustumCulling = GetComponent<FrustumCulling>();
        _frustumCulling.OnEnterCameraFrustum += OnEnterCameraFrustum;
        _frustumCulling.OnExitCameraFrustum += OnExitCameraFrustum;
        _doorInteraction = transform.parent.GetComponent<DoorInteraction>();
    }

    private void OnDestroy()
    {
        _frustumCulling.OnEnterCameraFrustum -= OnEnterCameraFrustum;
        _frustumCulling.OnExitCameraFrustum -= OnExitCameraFrustum;
    }

    private void OnEnterCameraFrustum()
    {
        _doorInteraction.StopAllDoorCoroutines();
        StartCoroutine(_doorInteraction.CloseDoor());
        // _meshRenderer.material.color = Color.green;
    }

    private void OnExitCameraFrustum()
    {
        _doorInteraction.StopAllDoorCoroutines();
        StartCoroutine(_doorInteraction.OpenDoor());
        // _meshRenderer.material.color = Color.red;
    }
}
