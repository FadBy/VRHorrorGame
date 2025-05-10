using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField, MinValue(0f)] private float _sensitivity = 0.5f;
    [SerializeField, Range(0f, 90f)] private float _verticalAngleCap = 80f;
    [SerializeField, Required] private TransformOffset _cameraOffset;
    
    private TransformOffset CameraOffset => _cameraOffset;
    
    private Vector2 Sensitivity => _sensitivity * Vector2.one;

    private float DownVerticalAngleCap => _verticalAngleCap;
    
    private float UpVerticalAngleCap => _verticalAngleCap;


    private PlayerInputAction _playerInputAction;
    private CursorState _cursorState;
    
    [Inject]
    private void Construct(CursorState cursorState, PlayerInputAction playerInputAction)
    {
        _playerInputAction = playerInputAction;
        _cursorState = cursorState;
    }

    private void OnEnable()
    {
        _playerInputAction.Player.CameraRotating.performed += OnPlayerInput;
    }

    private void OnDisable()
    {
        _playerInputAction.Player.CameraRotating.performed -= OnPlayerInput;
    }

    private void Start()
    {
        _cursorState.LockCursor();
        _cursorState.HideCursor();
    }

    private void OnPlayerInput(InputAction.CallbackContext context)
    {
        if (CameraOffset is null) return;
        if (Time.timeScale == 0) return;
        var mouseDelta = context.ReadValue<Vector2>();
        float yMouseDelta = -mouseDelta.y * Sensitivity.y;
        float xMouseDelta = mouseDelta.x * Sensitivity.x;
        
        var targetEulerAngles = CameraOffset.Rotation.eulerAngles;
        targetEulerAngles += new Vector3(yMouseDelta, xMouseDelta, 0f);
        if (targetEulerAngles.x > DownVerticalAngleCap && targetEulerAngles.x < 180f)
        {
            targetEulerAngles.x = DownVerticalAngleCap;
        }
        else if (targetEulerAngles.x < 360f - UpVerticalAngleCap && targetEulerAngles.x > 180f)
        {
            targetEulerAngles.x = 360f - UpVerticalAngleCap;
        }
        CameraOffset.Rotation = Quaternion.Euler(targetEulerAngles);
    }
}
