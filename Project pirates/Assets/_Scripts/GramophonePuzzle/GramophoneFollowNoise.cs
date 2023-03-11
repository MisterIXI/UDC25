using UnityEngine;

public class GramophoneFollowNoise : MonoBehaviour
{
    [field: SerializeField] private MeshRenderer _gramophoneMesh;
    [field: SerializeField] private MeshRenderer _plateMesh;
    private LevelNodeData _currentNodeData;
    private GramophoneMarker _currentTarget;
    private GramophoneSettings _gramophoneSettings;
    private void OnEnable()
    {
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
        _currentTarget = _currentNodeData.anchorList.GetComponentInChildren<GramophoneMarker>();
        if (_currentTarget == null)
        {
            Debug.LogError("No GramophoneMarker found in anchorlist");
        }
        CheckVisibility();
    }
    private void CheckVisibility()
    {
        if (_currentTarget != null && _currentTarget.ShowMesh)
        {
            _gramophoneMesh.enabled = true;
            _plateMesh.enabled = true;
        }
        else
        {
            _gramophoneMesh.enabled = false;
            _plateMesh.enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (_gramophoneSettings.DrawDebugGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            if (_currentTarget != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, _currentTarget.transform.position);
                Gizmos.DrawSphere(_currentTarget.transform.position, 0.15f);
            }
        }
    }
}