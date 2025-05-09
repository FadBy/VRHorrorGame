using UnityEngine;
using Zenject;

public class HandSway : MonoBehaviour
{
    [SerializeField] private float _step = 0.01f;
    [SerializeField] private TransformOffset _handOffset;
    
    private PlayerInputAction _playerInputAction;

    private Vector2 _lookInput;
    
    [Inject]
    private void Construct(PlayerInputAction playerInputAction)
    {
        _playerInputAction = playerInputAction;
    }
    
    private void Update()
    {
        ReadInputValue();
        SwayPosition();
    }

    private void ReadInputValue()
    {
        _lookInput = _playerInputAction.Player.CameraRotating.ReadValue<Vector2>();
    }

    private void SwayPosition()
    {
        var invertLook = _lookInput * -_step;
    }
}
