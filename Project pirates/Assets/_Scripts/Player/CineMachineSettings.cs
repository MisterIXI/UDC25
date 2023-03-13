using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CineMachineSettings : CinemachineExtension
{
    private PlayerController playercontroller;
    [SerializeField][HideInInspector] private Vector3 startingRotation; // ROTATION
    [SerializeField][HideInInspector] private float horizontalSpeed;
    [SerializeField][HideInInspector] private float verticalSpeed;
    [SerializeField][HideInInspector] private float clampAngleUp;
    [SerializeField][HideInInspector] private float clampAngleDown;
    [SerializeField][HideInInspector] private bool IsKeyboardAndMouseActive;
    private bool IsYAxisInverted;
    private PlayerSettings playerSettings;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        playercontroller = PlayerController.Instance;
        playerSettings = SettingsManager.PlayerSettings;
        clampAngleUp = playerSettings.CameraClampAngleUp;
        clampAngleDown = playerSettings.CameraClampAngleDown;
        IsKeyboardAndMouseActive = InputManager.IsKeyboardAndMouse;
        InputManager.OnControlSchemeChanged += OnControlSchemeChanged;
        SettingsManager.OnMouseLookSensitivityChanged += OnSensitivityChange;
        SettingsManager.OnGamepadLookSensitivityChanged += OnSensitivityChange;
        SettingsManager.OnInvertYAxisChanged += OnInvertChanged;
        // intialize with values from settings
        OnSensitivityChange(0);
        OnInvertChanged(playerSettings.InvertYAxis);
    }

    private void OnControlSchemeChanged()
    {
        IsKeyboardAndMouseActive = InputManager.IsKeyboardAndMouse;
        OnSensitivityChange(0);
    }
    private void OnInvertChanged(bool value)
    {
        IsYAxisInverted = value;
    }
    private void OnSensitivityChange(float value)
    {
        if (IsKeyboardAndMouseActive)
        {
            horizontalSpeed = playerSettings.MouseLookSensitivity * playerSettings.MouseSensitivityMultiplier;
            verticalSpeed = playerSettings.MouseLookSensitivity * playerSettings.MouseSensitivityMultiplier;
        }
        else
        {
            horizontalSpeed = playerSettings.GamepadLookSensitivity * playerSettings.GamepadSensitivityMultiplier;
            verticalSpeed = playerSettings.GamepadLookSensitivity * playerSettings.GamepadSensitivityMultiplier;
        }
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (playercontroller != null)
                {
                    // Debug.Log(playercontroller.GetDelta());
                    if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = playercontroller.GetDelta();
                    startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, clampAngleDown, clampAngleUp);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        InputManager.OnControlSchemeChanged -= OnControlSchemeChanged;
        base.OnDestroy();
    }
}
