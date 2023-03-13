using System;
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

    [SerializeField] Transform groundCheck;
    [SerializeField] float stepHeight = 0.5f;

    Ladder ladder;

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
        PlayerCameraController.Instance.SetCameraTarget(FollowTarget);
    }
    private void SubscribeToInput()
    {
        InputManager.OnLook += OnLookInput;
        InputManager.OnMove += OnMoveInput;
        InputManager.OnInteract += OnInteractInput;
    }
    private void FixedUpdate()
    {
        HandleMove();
        HandleLook();
    }


    private void HandleMove()
    {
        Vector3 newPos = Vector3.zero;

        _moveVector = Vector3.MoveTowards(_moveVector, _moveInput, _playerSettings.MovementSpeed * Time.deltaTime / _playerSettings.MovementSnapSeconds);
        if (_isMoving && !_snappedToLadder)
        {
            newPos = transform.TransformDirection(new Vector3(_moveVector.x, -1, _moveVector.y));
        }
        else if (_isMoving && _snappedToLadder)
        {
            newPos = transform.TransformDirection(new Vector3(0, _moveVector.y, 0));
        }
        _rigidbody.velocity = newPos * _playerSettings.MovementSpeed;
        StepClimb();
    }

    private void StepClimb()
    {
        // if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.2f))
        // {
        //     if (_isMoving && hit.collider.tag == "Stair")
        //     {
        //         // _rigidbody.position -= new Vector3(0, -stepHeight, 0);
        //         var velocity = _rigidbody.velocity;
        //         velocity.y += _playerSettings.StairPushForce;
        //         _rigidbody.velocity = velocity;
        //     }
        // }
        if (_IsOnStairs)
        {
            var velocity = _rigidbody.velocity;
            velocity.y += _playerSettings.StairPushForce;
            _rigidbody.velocity = velocity;
        }
    }
    private bool _IsOnStairs = false;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Stair")
        {
            Debug.Log("Entered Stairs");

            _IsOnStairs = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Stair")
        {
            Debug.Log("Exited Stairs");
            _IsOnStairs = false;
        }
    }
    private void HandleLook()
    {

    }

    public void SetLadderSnap(bool isOnLadder, Ladder ladder)
    {
        this.ladder = ladder;
        _isOnLadder = isOnLadder;
        Debug.Log(isOnLadder);
        if (isOnLadder)
        {
            _snappedToLadder = true;
            if (Vector3.Distance(transform.position, ladder.GetStartPoint().position) > Vector3.Distance(transform.position, ladder.GetEndPoint().position))
                transform.position = ladder.GetEndPoint().position;
            else
                transform.position = ladder.GetStartPoint().position;
        }
        else
        {
            _snappedToLadder = false;
            _isOnLadder = false;
            _snappedToLadder = false;
            _rigidbody.useGravity = true;
            this.ladder.IsPlayerOnLadder(false);
            this.ladder = null;
        }
    }

    private void UnsubscribeFromInput()
    {
        InputManager.OnLook -= OnLookInput;
        InputManager.OnMove -= OnMoveInput;
        InputManager.OnInteract -= OnInteractInput;
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
        //Debug.Log("Delta: " + _lookInputDelta);
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

    private void OnInteractInput(InputAction.CallbackContext context)
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
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        UnsubscribeFromInput();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Ladder"))
        {
            ladder = null;
            _isOnLadder = false;
            _snappedToLadder = false;
            _rigidbody.useGravity = true;
        }
    }
}
