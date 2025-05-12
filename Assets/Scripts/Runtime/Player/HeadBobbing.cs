using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class HeadBobbing : MonoBehaviour
{
    [SerializeField, Required] private TransformOffset _cameraOffset;
    [SerializeField, Required] private PlayerMovement _playerMovement;

    [SerializeField, MinValue(0f)] private float _intensity;
    [SerializeField, MinValue(0f)] private float _amplitude;
    [SerializeField] private float _thresholdY = -0.02f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepClips;

    [SerializeField, ReadOnly]
    private float _sinTime;

    private float _previousYOffset;

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

        float yOffset = -Mathf.Abs(Mathf.Sin(_sinTime) * _amplitude);
        _cameraOffset.Offset = new Vector3(0f, yOffset, 0f);

        if (_previousYOffset > _thresholdY && yOffset <= _thresholdY)
        {
            PlayFootstepSound();
        }

        _previousYOffset = yOffset;
    }

    private void PlayFootstepSound()
    {
        if (_footstepClips.Length == 0 || _audioSource == null) return;

        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(_footstepClips[Random.Range(0, _footstepClips.Length)]);
    }
}