using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FrustumCulling))]
public class FrustumCullingTeleport : MonoBehaviour
{
    [SerializeField] Vector3[] predefinedTeleportPoints;
    Vector3 currentPos;
    Vector3 newPos;
    private FrustumCulling _frustumCulling;
    private void Awake()
    {
        _frustumCulling = GetComponent<FrustumCulling>();
        _frustumCulling.OnEnterCameraFrustum += SelectNextTeleportPos;
        _frustumCulling.OnExitCameraFrustum += FrustumTeleport;
    }

    private void OnDestroy()
    {
        _frustumCulling.OnEnterCameraFrustum -= SelectNextTeleportPos;
        _frustumCulling.OnExitCameraFrustum -= FrustumTeleport;
    }
    

    public void FrustumTeleport()
    {
        currentPos = newPos;
        transform.position = currentPos;
        while (_frustumCulling.IsCurrentlyInCameraFrustum())
        {
            currentPos = newPos;
            transform.position = currentPos;
            SelectNextTeleportPos();
        }
    }

    public void SelectNextTeleportPos()
    {
        while(currentPos == newPos)
        {
            newPos = predefinedTeleportPoints[Random.Range(0, predefinedTeleportPoints.Length)];
        }
    }

}
