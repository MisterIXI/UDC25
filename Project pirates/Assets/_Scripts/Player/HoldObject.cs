using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class HoldObject : MonoBehaviour
{
    private Rigidbody _currentRigidbody;
    public Rigidbody PotentialRigidbody { get; private set; }
    public static event Action OnPotentialRigidbodyChanged;
    public static bool IsHoldingObject;
    private Transform _cameraTransform;
    private PlayerSettings _playerSettings;
    private float _holdObjectDistance;
    [field: SerializeField] private bool _drawDebugGizmos { get; set; }
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _playerSettings = SettingsManager.PlayerSettings;
        InputManager.OnHoldObject += OnHoldObjectInput;
    }
    private void FixedUpdate()
    {
        if (!IsHoldingObject)
        {
            //raycast to see if we can pick up an object
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, _playerSettings.HoldObjectMaxDistance))
            {
                if (hit.collider.TryGetComponent(out Rigidbody rigidbody))
                {
                    PotentialRigidbody = rigidbody;
                    OnPotentialRigidbodyChanged?.Invoke();
                }
            }
            else
            {
                PotentialRigidbody = null;
                OnPotentialRigidbodyChanged?.Invoke();
            }
        }
        else
        {
            // move object to fixed point in front of camera
            Vector3 targetPoint = _cameraTransform.position + _cameraTransform.forward * _holdObjectDistance;
            Vector3 targetVelocity = targetPoint - _currentRigidbody.transform.position;
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, _playerSettings.HoldObjectMaxSpeedMagnitude);
            _currentRigidbody.velocity = targetVelocity;
        }
    }
    private void OnHoldObjectInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (PotentialRigidbody != null)
            {
                _currentRigidbody = PotentialRigidbody;
                IsHoldingObject = true;
                _holdObjectDistance = Vector3.Distance(transform.position, _currentRigidbody.transform.position);
            }
        }
        else if (context.canceled)
        {
            _currentRigidbody = null;
            IsHoldingObject = false;
        }
    }
    private void SubscribeToInput() { InputManager.OnHoldObject += OnHoldObjectInput; }
    private void UnsubscribeToInput() { InputManager.OnHoldObject -= OnHoldObjectInput; }
    private void OnDestroy()
    {
        UnsubscribeToInput();
    }
    private void OnDrawGizmos()
    {
        if (_drawDebugGizmos)
        {
            if (_cameraTransform != null && _playerSettings != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(_cameraTransform.position, _cameraTransform.forward * _playerSettings.HoldObjectMaxDistance);
            }
        }
    }
}