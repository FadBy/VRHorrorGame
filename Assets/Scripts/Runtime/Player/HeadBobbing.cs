using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField, Required] private TransformOffset _cameraOffset;
    [SerializeField, Required] private PlayerMovement _playerMovement;
    
    [SerializeField, MinValue(0f)] private float _intensity;
    [SerializeField, MinValue(0f)] private float _amplitude;
    
    [SerializeField, ReadOnly]
    private float _sinTime;
    
    private void Update()
    {
        if (_playerMovement.LastWalkedDistance > 0f)
        {
            _sinTime += Time.deltaTime * _intensity;
        }
        else if (_playerMovement.LastWalkedDistance == 0f)
        {
            _sinTime = 0f;
        }

        var yOffset = -Mathf.Abs(Mathf.Sin(_sinTime) * _amplitude);
        _cameraOffset.Offset = new Vector3(0f, yOffset, 0f);
    }
}
