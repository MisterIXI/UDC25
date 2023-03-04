using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CineMachineSettings : CinemachineExtension
{
    private PlayerController playercontroller;
    private Vector3 startingRotation; // ROTATION
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngle = 80f;
    protected override void Awake()
    {
        playercontroller = PlayerController.Instance;
        base.Awake();
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (playercontroller != null)
                {

                    Debug.Log(playercontroller.GetDelta());
                    if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = playercontroller.GetDelta();
                    startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, -clampAngle);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                }
            }
        }
    }
}
