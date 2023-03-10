using System;
using UnityEngine;
public class CameraBobbing : MonoBehaviour
{
    [field: SerializeField] private bool DrawDebugGizmos { get; set; }
    public static event Action OnStepTaken;
    private PlayerSettings _playerSettings;
    [SerializeField] private float _bobbingValue = 1f;
    [SerializeField] private bool _bobbingLeft = true;
    private Rigidbody _playerRigidbody;
    [SerializeField] private float _bobbingAccumulator;
    private void Start()
    {
        _playerSettings = SettingsManager.PlayerSettings;
        _playerRigidbody = PlayerController.Instance.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float playerXYVelocity = new Vector2(_playerRigidbody.velocity.x, _playerRigidbody.velocity.z).magnitude;
        if (playerXYVelocity > 0f)
        {
            float oldValue = _bobbingValue;
            _bobbingAccumulator += playerXYVelocity * Time.deltaTime;
            _bobbingValue = Mathf.PingPong(_bobbingAccumulator / _playerSettings.StepLength, 1);
            if (_bobbingLeft && _bobbingValue > oldValue)
            { // switched from left to right direction
                _bobbingLeft = false;
                OnStepTaken?.Invoke();
            }
            else if (!_bobbingLeft && _bobbingValue < oldValue)
            {   // switched from right to left direction
                _bobbingLeft = true;
                OnStepTaken?.Invoke();
            }
            transform.localPosition = CalculatePosition(_bobbingValue);
        }
    }

    private Vector3 CalculatePosition(float value)
    {

        float curveX = _playerSettings.CameraBobbingCurveX.Evaluate(value);
        curveX *= _playerSettings.CameraBobbingMultX;
        float curveY = _playerSettings.CameraBobbingCurveY.Evaluate(value);
        curveY *= _playerSettings.CameraBobbingMultY;
        curveY += _playerSettings.CameraVerticalOffset;
        return new Vector3(curveX, curveY, 0f);
    }

    private void OnDrawGizmos()
    {
        if (DrawDebugGizmos && _playerSettings != null)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < 100; i++)
            {
                Gizmos.DrawSphere(transform.parent.position + CalculatePosition(i / 100f), 0.01f);
            }

        }
    }
}