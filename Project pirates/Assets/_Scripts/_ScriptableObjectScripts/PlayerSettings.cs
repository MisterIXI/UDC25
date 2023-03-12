using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Project pirates/PlayerSettings", order = 0)]
public class PlayerSettings : ScriptableObject
{
    [field: Header("General")]
    [field: SerializeField] public bool CloneScriptableObjectOnStart { get; private set; } = true;
    [field: Header("Volume")]
    [field: SerializeField][field: Range(0.01f, 1)] public float MasterVolume { get; set; } = 0.3f;
    [field: SerializeField][field: Range(0.01f, 1)] public float MusicVolume { get; set; } = 1f;
    [field: SerializeField][field: Range(0.01f, 1)] public float SfxVolume { get; set; } = 1f;
    [field: Header("Controls")]
    [field: SerializeField][field: Range(0.01f, 1)] public float MouseLookSensitivity { get; set; } = 1f;
    [field: SerializeField][field: Range(0.01f, 1)] public float GamepadLookSensitivity { get; set; } = 1f;
    [field: SerializeField] public bool InvertYAxis { get; set; } = false;
    [field: SerializeField] public float SensitivityMultiplier { get; set; } = 10f;
    [field: Header("Movement")]
    [field: SerializeField] public float MovementSpeed { get; set; } = 1f;
    [field: SerializeField][field: Range(0.01f, 1f)] public float MovementSnapSeconds { get; set; } = 1f;
    [field: SerializeField] public float StepLength { get; set; } = 50f;
    [field: SerializeField] public float StairPushForce { get; set; } = 0.5f;
    [field: Header("Camera")]
    [field: SerializeField][field: Range(-1f, 2f)] public float CameraVerticalOffset { get; set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 1f)] public float CameraBobbingMultX { get; set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 1f)] public float CameraBobbingMultY { get; set; } = 0.5f;
    [field: SerializeField] public AnimationCurve CameraBobbingCurveX { get; set; } = AnimationCurve.Constant(0, 1, 1);
    [field: SerializeField] public AnimationCurve CameraBobbingCurveY { get; set; } = AnimationCurve.Constant(0, 1, 1);
    [field: SerializeField][field: Range(0.01f, 89.99f)] public float CameraClampAngleUp { get; set; } = 80f;
    [field: SerializeField][field: Range(0.01f, -89.99f)] public float CameraClampAngleDown { get; set; } = -80f;
    [field: Header("HoldObject")]
    [field: SerializeField] public float HoldObjectMoveForce { get; set; } = 100f;
    [field: SerializeField] public float HoldObjectMaxDistance { get; set; } = 3f;
    [field: Header("Interact")]
    [field: SerializeField] public float InteractMaxDistance { get; set; } = 3f;
    [field: SerializeField] public float InteractMoveForce { get; set; } = 100f;
    [field: SerializeField] public float InteractThrowMagnitude { get; set; } = 5f;
    [field: Header("Inventory")]
    [field: SerializeField] public Vector3 InvItemRotation { get; set; } = new Vector3(-70f, -10f, -20f);
    [field: SerializeField] public Vector3 InvItemPosition { get; set; } = new Vector3(-0.6f, -0.2f, 1.2f);
    [field: SerializeField] public float InvRubberbandForce { get; set; } = 100f;
    [field: SerializeField] public float InvRotationSpeed { get; set; } = 3f;
    [field: Header("Compass Riddle")]
    [field: SerializeField][field: Range(0f, 0.1f)] public float FogDensity { get; set; } = 0.06f;
    [field: SerializeField][field: Range(0f, 1f)] public float SunIntensity { get; set; } = 1f;
    [field: Header("UI")]
    [field: SerializeField][field: Range(0f, 0.1f)] public float UiTextCharacterTweening { get; set; } = 0.025f;
    [field: SerializeField][field: Range(0f, 1f)] public float UiSideSlideTween { get; set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 1f)] public float UiVertSlideTween { get; set; } = 1f;
}