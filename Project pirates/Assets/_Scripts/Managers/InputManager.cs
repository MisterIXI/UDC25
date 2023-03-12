using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInput _playerInput;
    public static bool IsKeyboardAndMouse => Instance._playerInput.currentControlScheme != "Gamepad";
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onControlsChanged += OnSchemeChange;
        SubscribeToInput();
        DontDestroyOnLoad(gameObject);
    }
    public static event Action OnControlSchemeChanged;
    public static event Action<CallbackContext> OnLook;
    public static event Action<CallbackContext> OnMove;
    public static event Action<CallbackContext> OnInteract;
    public static event Action<CallbackContext> OnPauseGame;
    public static event Action<CallbackContext> OnHoldObject;

    private void OnSchemeChange(PlayerInput playerInput)
    {
        OnControlSchemeChanged?.Invoke();
    }

    private void OnLookInput(CallbackContext context)
    {
        OnLook?.Invoke(context);
    }

    private void OnMoveInput(CallbackContext context)
    {
        OnMove?.Invoke(context);
    }

    private void OnInteractInput(CallbackContext context)
    {
        OnInteract?.Invoke(context);
    }


    private void OnHoldObjectInput(CallbackContext context)
    {
        OnHoldObject?.Invoke(context);
    }

    private void OnPauseGameInput(CallbackContext context)
    {
        OnPauseGame?.Invoke(context);
    }

    private void SubscribeToInput()
    {
        _playerInput.actions["Look"].started += OnLookInput;
        _playerInput.actions["Look"].performed += OnLookInput;
        _playerInput.actions["Look"].canceled += OnLookInput;

        _playerInput.actions["Move"].started += OnMoveInput;
        _playerInput.actions["Move"].performed += OnMoveInput;
        _playerInput.actions["Move"].canceled += OnMoveInput;

        _playerInput.actions["Interact"].started += OnInteractInput;
        _playerInput.actions["Interact"].performed += OnInteractInput;
        _playerInput.actions["Interact"].canceled += OnInteractInput;

        _playerInput.actions["PauseGame"].started += OnPauseGameInput;
        _playerInput.actions["PauseGame"].performed += OnPauseGameInput;
        _playerInput.actions["PauseGame"].canceled += OnPauseGameInput;

        _playerInput.actions["HoldObject"].started += OnHoldObjectInput;
        _playerInput.actions["HoldObject"].performed += OnHoldObjectInput;
        _playerInput.actions["HoldObject"].canceled += OnHoldObjectInput;
    }

    private void UnsubscribeFromInput()
    {
        _playerInput.actions["Look"].started -= OnLookInput;
        _playerInput.actions["Look"].performed -= OnLookInput;
        _playerInput.actions["Look"].canceled -= OnLookInput;

        _playerInput.actions["Move"].started -= OnMoveInput;
        _playerInput.actions["Move"].performed -= OnMoveInput;
        _playerInput.actions["Move"].canceled -= OnMoveInput;

        _playerInput.actions["Interact"].started -= OnInteractInput;
        _playerInput.actions["Interact"].performed -= OnInteractInput;
        _playerInput.actions["Interact"].canceled -= OnInteractInput;

        _playerInput.actions["PauseGame"].started -= OnPauseGameInput;
        _playerInput.actions["PauseGame"].performed -= OnPauseGameInput;
        _playerInput.actions["PauseGame"].canceled -= OnPauseGameInput;

        _playerInput.actions["HoldObject"].started -= OnHoldObjectInput;
        _playerInput.actions["HoldObject"].performed -= OnHoldObjectInput;
        _playerInput.actions["HoldObject"].canceled -= OnHoldObjectInput;
    }
    private void OnDestroy()
    {
        UnsubscribeFromInput();
    }

    [ContextMenu("Debug print controlscheme")]
    private void DebugPrintControlscheme()
    {
        Debug.Log("Is keyboard and mouse: " + IsKeyboardAndMouse);
    }
}