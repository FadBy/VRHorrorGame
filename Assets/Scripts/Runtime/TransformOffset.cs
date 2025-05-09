using UnityEngine;

public class TransformOffset : MonoBehaviour
{
    [SerializeField] private Vector3 _originLocalPosition;
    private Vector3 _offset;
    
    public Vector3 Offset
    {
        get => _offset;

        set
        {
            _offset = value;
            SyncPosition();
        }
    }

    public Vector3 OriginLocalPosition
    {
        get => _originLocalPosition;
        
        set
        {
            _originLocalPosition = value;
            SyncPosition();
        }
    }

    public Quaternion Rotation
    {
        get => transform.rotation;

        set => transform.rotation = value;
    }

    private void SyncPosition()
    {
        transform.localPosition = _originLocalPosition + _offset;
    }
}
