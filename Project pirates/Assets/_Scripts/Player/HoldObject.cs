using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class HoldObject : MonoBehaviour
{
    private Rigidbody _currentRigidbody;
    public static Rigidbody PotentialRigidbody { get; private set; }
    public static event Action OnPotentialRigidbodyChanged;
    public static bool IsHoldingObject;
    private Transform _cameraTransform;
    private PlayerSettings _playerSettings;
    private float _holdObjectDistance;
    private PlayerInventory _playerInventory;
    [field: SerializeField] private bool _drawDebugGizmos { get; set; }
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _playerInventory = GetComponent<PlayerInventory>();
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
                if (hit.rigidbody != null && hit.rigidbody.tag != "KeyItem")
                {
                    if (PotentialRigidbody != hit.rigidbody)
                    {
                        PotentialRigidbody = hit.rigidbody;
                        OnPotentialRigidbodyChanged?.Invoke();
                    }
                }
                else
                {
                    if (PotentialRigidbody != null)
                    {
                        PotentialRigidbody = null;
                        OnPotentialRigidbodyChanged?.Invoke();
                    }
                }
            }
            else
            {
                if (PotentialRigidbody != null)
                {
                    PotentialRigidbody = null;
                    OnPotentialRigidbodyChanged?.Invoke();
                }
            }
        }
        else
        {
            // move object to fixed point in front of camera
            Vector3 targetPoint = _cameraTransform.position + _cameraTransform.forward * _holdObjectDistance;
            Vector3 targetVelocity = targetPoint - _currentRigidbody.transform.position;
            targetVelocity = targetVelocity * _playerSettings.HoldObjectMoveForce;
            _currentRigidbody.AddForce(targetVelocity);
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
                UpdateRigidbody(_currentRigidbody, true);
                OnPotentialRigidbodyChanged?.Invoke();
            }
        }
        else if (context.canceled)
        {
            if (_currentRigidbody != null && _playerInventory.Item?.GetComponent<Rigidbody>() != _currentRigidbody)
                UpdateRigidbody(_currentRigidbody, false);
            _currentRigidbody = null;
            IsHoldingObject = false;
            OnPotentialRigidbodyChanged?.Invoke();

        }
    }
    private void UpdateRigidbody(Rigidbody rigidbody, bool isHeld)
    {
        if (isHeld)
        {
            rigidbody.useGravity = false;
            rigidbody.drag = 10f;
        }
        else
        {
            rigidbody.useGravity = true;
            rigidbody.drag = 1f;
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