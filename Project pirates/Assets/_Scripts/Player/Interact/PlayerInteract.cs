using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [field: SerializeField] private bool _drawDebugGizmos;
    public static event Action OnInteractableTargetChanged;
    public static IInteractable PossibleInteractableObject { get; private set; }
    public static IInteractable CurrentInteractableObject { get; private set; }
    private Camera _mainCamera;
    private PlayerInventory _playerInventory;
    private PlayerSettings _playerSettings;

    private void SubscribeToInput() { InputManager.OnInteract += OnInteract; }
    private void UnsubscribeToInput() { InputManager.OnInteract -= OnInteract; }
    private void Awake()
    {
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
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _playerSettings.InteractMaxDistance))
        {
            IInteractable interactableObject = null;
            if (hit.rigidbody != null)
                interactableObject = hit.rigidbody.GetComponent<IInteractable>();
            else if (interactableObject == null)
                interactableObject = hit.collider.GetComponent<IInteractable>();
            if (PossibleInteractableObject != interactableObject)
            {
                PossibleInteractableObject = interactableObject;
                OnInteractableTargetChanged?.Invoke();
            }
        }
        else
        {
            if (PossibleInteractableObject != null)
            {
                PossibleInteractableObject = null;
                OnInteractableTargetChanged?.Invoke();
            }
        }
    }

    private void InteractWithObject(IInteractable interactableObject)
    {
        interactableObject?.Interact();
    }

    public void DropObject()
    {
        ((MonoBehaviour)CurrentInteractableObject).transform.parent = null;
        CurrentInteractableObject = null;
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (PossibleInteractableObject != null)
            {
                InteractWithObject(PossibleInteractableObject);
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
                Gizmos.DrawLine(_mainCamera.transform.position, _mainCamera.transform.position + _mainCamera.transform.forward * _playerSettings.InteractMaxDistance);
            }
        }
    }

}