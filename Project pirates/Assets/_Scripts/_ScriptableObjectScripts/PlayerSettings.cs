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
    [field: SerializeField] public float MasterVolume { get; private set; } = 0.3f;
    [field: SerializeField] public float MusicVolume { get; private set; } = 1f;
    [field: SerializeField] public float SfxVolume { get; private set; } = 1f;
    [field: Header("Controls")]
    [field: SerializeField] public float MouseLookSensitivity { get; private set; } = 1f;
    [field: SerializeField] public float GamepadLookSensitivity { get; private set; } = 1f;
    [field: SerializeField] public bool InvertYAxis { get; private set; } = false;
    [field: Header("Movement")]
    [field: SerializeField] public float MovementSpeed { get; private set; } = 1f;
    [field: SerializeField][field: Range(0.01f,1f)] public float MovementSnapSpeed { get; private set; } = 1f;
    [field: Header("Camera")]
    [field: SerializeField] public float CameraMinX { get; private set; } = 1f;
    [field: SerializeField] public float CameraMaxX { get; private set; } = 1f;


}