using UnityEngine;

public class GramophoneFollowNoise : MonoBehaviour
{
    [field: SerializeField] private LevelNodeData _currentNodeData;

    [field: SerializeField] private GramophoneMarker _currentTarget;
    private bool _isFinalized;
    private GramophoneSettings _gramophoneSettings;
    private AudioSource _audioSource;
    private void OnEnable()
    {
        Debug.Log($"GramophoneFollowNoise enabled");
        // Debug.Log($"Gameobject active: {gameObject.activeInHierarchy}");
        _gramophoneSettings = SettingsManager.GramophoneSettings;
        transform.parent = null;
        LevelOrchestrator.OnCurrentNodeChanged += ChangeCurrentTarget;
        ChangeCurrentTarget();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = SoundManager.AudioClips.GrammophonMusic;
        _audioSource.loop = true;
        _audioSource.Play();
        // SettingsManager.OnSFXVolumeChanged += OnVolumeSettingsChanged;
        // SettingsManager.OnMasterVolumeChanged += OnVolumeSettingsChanged;
    }
    private void FixedUpdate()
    {
        if (!_isFinalized)
            LerpToCurrentTarget();
        else
            LerpToFinalPosition();
    }
    private void LerpToFinalPosition()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.fixedDeltaTime * _gramophoneSettings.GramoFollowSpeed * 0.3f);
        if (transform.localPosition == Vector3.zero)
        {
            gameObject.AddComponent<GramophoneBackwardsNoise>();
            Destroy(this);
        }
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
        if (_currentTarget.IsFinalMarker)
            FinalizeGramophone();
    }

    private void FinalizeGramophone()
    {
        Debug.Log("FinalizeGramophone");
        _isFinalized = true;
        LevelOrchestrator.OnCurrentNodeChanged -= ChangeCurrentTarget;
        transform.parent = Camera.main.transform;
        transform.localPosition = Vector3.zero;
    }
    private void OnVolumeSettingsChanged(float newVolume)
    {
        _audioSource.volume = SettingsManager.PlayerSettings.TotalMusicVolume;
    }
    private void OnDestroy()
    {
        LevelOrchestrator.OnCurrentNodeChanged -= ChangeCurrentTarget;
        // SettingsManager.OnSFXVolumeChanged -= OnVolumeSettingsChanged;
        // SettingsManager.OnMasterVolumeChanged -= OnVolumeSettingsChanged;
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