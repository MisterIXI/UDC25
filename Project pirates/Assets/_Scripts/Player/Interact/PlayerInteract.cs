using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [field: SerializeField] private bool _drawDebugGizmos;
    public static event Action currentInteractableObjectChanged;
    public static IInteractable PossibleInteractableObject { get; private set; }
    public static IInteractable CurrentInteractableObject { get; private set; }
    private Rigidbody _currentInteractRigidbody;
    private Camera _mainCamera;
    private PlayerInventory _playerInventory;
    private PlayerSettings _playerSettings;
    [field: SerializeField] private Transform _interactObjectHoldPosition;

    private void SubscribeToInput() { InputManager.OnInteract += OnInteract; }
    private void UnsubscribeToInput() { InputManager.OnInteract -= OnInteract; }
    private void Awake() {
        _playerInventory = GetComponent<PlayerInventory>();
    }
    void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        _mainCamera = Camera.main;
        // ui = GameObject.FindGameObjectWithTag("UI_Interact");
        // text = ui.GetComponentInChildren<TMP_Text>();

        SubscribeToInput();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _mainCamera.transform.forward, out hit, _playerSettings.InteractMaxDistance))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactableObject))
            {
                PossibleInteractableObject = interactableObject;
                currentInteractableObjectChanged?.Invoke();
            }
            else
            {
                PossibleInteractableObject = null;
                currentInteractableObjectChanged?.Invoke();
            }
        }
        if (CurrentInteractableObject != null)
        {
            Vector3 moveVector = _interactObjectHoldPosition.position - ((MonoBehaviour)CurrentInteractableObject).transform.position;
            if(moveVector.magnitude > 0.1f)
            {
                _currentInteractRigidbody.AddForce(moveVector * _playerSettings.InteractMoveForce);
            }
        }
    }



    private void PickupObject(IInteractable interactableObject)
    {
        if (CurrentInteractableObject != null)
        {
            _currentInteractRigidbody.AddForce(_mainCamera.transform.forward * _playerSettings.InteractThrowMagnitude, ForceMode.Impulse);
        }
        CurrentInteractableObject = PossibleInteractableObject;
        _playerInventory.TakeObject(((MonoBehaviour)CurrentInteractableObject).gameObject);
        _currentInteractRigidbody = ((MonoBehaviour)CurrentInteractableObject).GetComponent<Rigidbody>();
    }

    public void DropObject()
    {
        ((MonoBehaviour)CurrentInteractableObject).transform.parent = null;
        CurrentInteractableObject = null;
        _currentInteractRigidbody = null;
        
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (PossibleInteractableObject != null)
            {
                PickupObject(PossibleInteractableObject);
            }
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToInput();
    }

    private void OnDrawGizmos()
    {
        if (_drawDebugGizmos)
        {
            if (_playerSettings != null && _mainCamera != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(_interactObjectHoldPosition.position, Vector3.one * 0.4f);
                Gizmos.DrawLine(transform.position, transform.position + _mainCamera.transform.forward * _playerSettings.InteractMaxDistance);
            }
        }
    }

}