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
    private bool _isOnLadder;
    private bool _snappedToLadder;
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
    }
    private void SubscribeToInput()
    {
        InputManager.OnLook += OnLookInput;
        InputManager.OnMove += OnMoveInput;
        InputManager.OnClimb += OnClimbInput;
    }
    private void FixedUpdate()
    {
        HandleMove();
        HandleLook();
    }


    private void HandleMove()
    {
        Vector3 newPos = Vector3.zero;
        if (_isMoving && !_snappedToLadder)
        {
            newPos = transform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y));
        }
        else if (_isMoving && _snappedToLadder)
        {
            newPos = transform.TransformDirection(new Vector3(0, _moveInput.y, 0));
        }
        _rigidbody.velocity = newPos * _playerSettings.MovementSpeed;
    }
    private void HandleLook()
    {

    }


    private void UnsubscribeFromInput()
    {
        InputManager.OnLook -= OnLookInput;
        InputManager.OnMove -= OnMoveInput;
        InputManager.OnClimb -= OnClimbInput;
    }
    private void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _lookInputDelta = context.ReadValue<Vector2>();
            // Debug.Log(_lookInputDelta);
        }
        else if (context.canceled)
        {
            _lookInputDelta = Vector2.zero;
        }
    }
    public Vector2 GetDelta()
    {
        Debug.Log("Delta: " + _lookInputDelta);
        if (_lookInputDelta != null)
        {
            return _lookInputDelta;
        }
        else
        {
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

    private void OnClimbInput(InputAction.CallbackContext context)
    {
        if (context.performed && _isOnLadder)
        {
            _snappedToLadder = !_snappedToLadder;
            if (_snappedToLadder)
            {
                _rigidbody.useGravity = false;
                _rigidbody.velocity = Vector3.zero;
            }
            else
            {
                _rigidbody.useGravity = true;
            }
            Debug.Log("snap to ladder: " + _snappedToLadder);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Ladder"))
        {
            _isOnLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Ladder"))
        {
            _isOnLadder = false;
            _snappedToLadder = false;
            _rigidbody.useGravity = true;
        }
    }
}
