using UnityEngine;

public class FlashlightMovement : MonoBehaviour
{
    [SerializeField] private Transform _targetRotation;
    [SerializeField] private float _rotationSpeed;

    private Vector3 _speed;
    private Vector3 _acceleration;

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation.rotation, _rotationSpeed * Time.deltaTime);
    }
}
