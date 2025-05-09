using UnityEngine;
using Zenject;

public class PlayerMovementNewInputReader : PlayerMovementInputReader
{
    private PlayerInputAction _playerInputAction;
    
    [Inject]
    public PlayerMovementNewInputReader(PlayerInputAction playerInputAction)
    {
        _playerInputAction = playerInputAction;
        playerInputAction.Player.Walking.Enable();
    }

    public override Vector2 PlayerMovement => _playerInputAction.Player.Walking.ReadValue<Vector2>();
}
