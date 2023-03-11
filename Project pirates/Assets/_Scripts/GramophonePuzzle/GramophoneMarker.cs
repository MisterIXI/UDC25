using UnityEngine;

public class GramophoneMarker : MonoBehaviour
{

    private GramophoneSettings _gramophoneSettings;
    private void Start()
    {
        _gramophoneSettings = SettingsManager.GramophoneSettings;
    }

    private void OnDrawGizmos()
    {
        if (_gramophoneSettings && _gramophoneSettings.DrawDebugGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.4f);
        }
    }
}