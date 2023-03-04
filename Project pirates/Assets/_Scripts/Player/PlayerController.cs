using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Vector2 _moveInput;
    private Vector2 _moveVector;
    private Vector2 _lookInputDelta;
    private Vector2 _lookVector;
    public static PlayerController Instance { get; private set; }
    private PlayerSettings _playerSettings => SettingsManager.PlayerSettings;
    private Rigidbody _rigidbody;
    [field: SerializeField] public Transform FollowTarget { get; private set; }
    private bool _isMoving = true;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            if (value)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            _isMoving = value;
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        SubscribeToInput();
    }

    private void Start()
    {
        _lookVector = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void SubscribeToInput()
    {
        InputManager.OnLook += OnLookInput;
        InputManager.OnMove += OnMoveInput;
    }
    private void FixedUpdate()
    {
        HandleMove();
        HandleLook();
    }


    private void HandleMove()
    {
        if (_isMoving)
        {
            Vector3 newPos =transform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y));
            _rigidbody.MovePosition(transform.position + newPos * _playerSettings.MovementSpeed * Time.deltaTime);
        }
    }
    private void HandleLook()
    {

    }


    private void UnsubscribeFromInput()
    {
        InputManager.OnLook -= OnLookInput;
        InputManager.OnMove -= OnMoveInput;
    }
    private void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _lookInputDelta = context.ReadValue<Vector2>();
            
        }
        else if (context.canceled)
        {
            _lookInputDelta = Vector2.zero;
        }
    }
    public Vector2 GetDelta(){
        if(_lookInputDelta != null)
        {
            return _lookInputDelta;
        }else{
            return Vector2.zero;
        }
    }
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            _moveInput = Vector2.zero;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }
}
