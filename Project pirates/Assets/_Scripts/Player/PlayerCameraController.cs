using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public static PlayerCameraController Instance { get; private set; }

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineInputProvider _inputProvider;
    private CinemachinePOV _pov;
    private Transform _playerTransform;
    private Transform _cameraTransform;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _pov = GetComponent<CinemachinePOV>();
        _cameraTransform = Camera.main.transform;
    }
    private void Start()
    {
        _playerTransform = PlayerController.Instance.transform;
        _virtualCamera.Follow = _playerTransform.GetComponentInChildren<CameraBobbing>().transform;
        if (_virtualCamera.Follow == null)
        {
            Debug.LogWarning("CameraBobbing not found in children of player. Falling back to player transform.");
            _virtualCamera.Follow = _playerTransform;
        }
    }

    private void FixedUpdate()
    {
        _playerTransform.forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up);
    }

    private void OnDestroy() {
        if (Instance == this)
            Instance = null;
    }
}