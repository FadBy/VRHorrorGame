using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SplineContainer _spline;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _fixingTime = 5;
    [SerializeField] private TVController _tvController;
    [SerializeField] private BillBoard _billBoard;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepClips;

    private float _currentSpeed;

    private Quaternion _initialRotation;

    public float CurrentSpeed
    {
        get => _currentSpeed;
        set
        {
            _animator.SetFloat("WalkValue", 0f);
            _currentSpeed = value;
        } 
    }
    
    [Button]
    public void OnInteract()
    {
        StartCoroutine(FixingCoroutine());
    }

    private IEnumerator FixingCoroutine()
    {
        _initialRotation = transform.rotation;
        _billBoard.Enabled = false;
        yield return FollowPath(false);
        _animator.SetTrigger("Fix");
        yield return new WaitForSeconds(_fixingTime);
        _tvController.Fix();
        yield return FollowPath(true);
        _billBoard.Enabled = true;
        transform.rotation = _initialRotation;

    }

    private IEnumerator FollowPath(bool flip = false)
    {
        CurrentSpeed = 0f;

        float t = flip ? 1f : 0f;
        float direction = flip ? -1f : 1f;
        float splineLength = _spline.Spline.GetLength();

        while ((flip && t > 0f) || (!flip && t < 1f))
        {
            Accelarate(_acceleration);
            _animator.SetFloat("WalkValue", CurrentSpeed / _maxSpeed);

            _spline.Spline.Evaluate(t, out var position, out var tangent, out var upVector);
            transform.position = _spline.transform.TransformPoint(position);

            if ((Vector3)tangent != Vector3.zero)
            {
                var lookDirection = flip ? tangent : -tangent;
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            t += direction * (CurrentSpeed / splineLength) * Time.deltaTime;

            yield return null;
        }

        CurrentSpeed = 0f;
    }

    private void Accelarate(float acceleration)
    {
        CurrentSpeed += acceleration * Time.deltaTime;
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0f, _maxSpeed);
    }

    public void PlayFootstepSound()
    {
        if (_footstepClips.Length == 0 || _audioSource == null) return;

        _audioSource.pitch = Random.Range(0.4f, 0.8f);
        _audioSource.PlayOneShot(_footstepClips[Random.Range(0, _footstepClips.Length)]);
    }
}