using UnityEngine;

public class FC_LevelGeometry : MonoBehaviour
{
    private FrustumCulling _frustumCulling;

    private void Awake()
    {
        _frustumCulling = GetComponent<FrustumCulling>();
    }
}