using UnityEngine;

public class GramophoneFollowNoise : MonoBehaviour
{
    [field: SerializeField] private LevelNodeData _currentNodeData;

    [field: SerializeField] private GramophoneMarker _currentTarget;
    private GramophoneSettings _gramophoneSettings;
    private void OnEnable()
    {
        Debug.Log($"Gameobject active: {gameObject.activeInHierarchy}");
        _gramophoneSettings = SettingsManager.GramophoneSettings;
        transform.parent = null;
        LevelOrchestrator.OnCurrentNodeChanged += ChangeCurrentTarget;
        ChangeCurrentTarget();
    }
    private void FixedUpdate()
    {
        LerpToCurrentTarget();
    }
    private void LerpToCurrentTarget()
    {
        if (_currentTarget != null && _currentTarget.transform.position != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.transform.position, Time.fixedDeltaTime * _gramophoneSettings.GramoFollowSpeed);
        }
    }
    private void ChangeCurrentTarget()
    {
        _currentNodeData = LevelOrchestrator.Instance.CurrentNode;
        _currentTarget = PrefabPool.GetInstantiatedAnchorList(_currentNodeData.GUID).GetComponentInChildren<GramophoneMarker>();
        if (_currentTarget == null)
        {
            Debug.LogError("No GramophoneMarker found in anchorlist");
        }
    }

    private void OnDrawGizmos()
    {
        if (_gramophoneSettings && _gramophoneSettings.DrawDebugGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.4f);
            if (_currentTarget != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, _currentTarget.transform.position);
                Gizmos.DrawWireCube(_currentTarget.transform.position, Vector3.one * 0.5f);
            }
        }
    }
}