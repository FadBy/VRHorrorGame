using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, MinValue(0f)] private float _defaultWalkingSpeed = 2f;
    [SerializeField, Required] private Transform _headTransform;
    [FormerlySerializedAs("_xInputSmoother")] [SerializeField, Required] private Vector2InputSmoother xVector2InputSmoother;
    [FormerlySerializedAs("_yInputSmoother")] [SerializeField, Required] private Vector2InputSmoother yVector2InputSmoother;
    [SerializeField] private Rigidbody _rb;

    private PlayerInputAction _playerInputAction;
    private PlayerMovementInputReader _playerMovementInputReader;

    private Vector2 _currentWalkingDirection;

    public float WalkingSpeed => _defaultWalkingSpeed;

    public float LastWalkedDistance { get; private set; }

    [Inject]
    private void Construct(PlayerInputAction playerInputAction, PlayerMovementInputReader playerMovementInputReader)
    {
        _playerInputAction = playerInputAction;
        _playerMovementInputReader = playerMovementInputReader;
    }
    
    private void Update()
    {
        var walkingVector = xVector2InputSmoother.Handle(ReadWalkInputAxis());
        SetWalkingDirection(walkingVector);
    }

    private void FixedUpdate()
    {
        Walk(Time.fixedDeltaTime);
    }

    private Vector2 ReadWalkInputAxis()
    {
        return _playerMovementInputReader.PlayerMovement;
    }

    private void SetWalkingDirection(Vector2 walkingDirection)
    {
        _currentWalkingDirection = walkingDirection;
    }

    private void Walk(float time)
    {
        var headYRotation = Quaternion.Euler(0, _headTransform.eulerAngles.y, 0);
        var headDirection = headYRotation * new Vector3(_currentWalkingDirection.x, 0f, _currentWalkingDirection.y);
        
        _rb.velocity = WalkingSpeed * headDirection;
        Debug.Log(_rb.velocity);
        
        var walkVector = time * WalkingSpeed * headDirection;
        LastWalkedDistance = walkVector.magnitude;
    }
}
