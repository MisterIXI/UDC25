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
    [field: SerializeField][field: Range(0.01f, 1f)] public float MovementSnapSpeed { get; set; } = 1f;
    [field: Header("Camera")]

    [field: SerializeField][field: Range(0.01f, 89.99f)] public float CameraClampAngleUp { get; set; } = 80f;
    [field: SerializeField][field: Range(0.01f, 89.99f)] public float CameraClampAngleDown { get; set; } = -80f;


}