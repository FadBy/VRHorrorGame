using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private TransformOffset _cameraOffset;
    [SerializeField] private LayerMask _layerMask;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HitRaycastables();
        }
    }

    private void HitRaycastables()
    {
        Raycast()?.OnRaycastHit();
    }

    private Raycastable Raycast()
    {
        if (Physics.Raycast(_cameraOffset.transform.position, _cameraOffset.transform.forward, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            if (hit.transform.TryGetComponent<Raycastable>(out var raycastable))
            {
                return raycastable;
            }
        }

        return null;
    }

    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_cameraOffset == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(_cameraOffset.transform.position, _cameraOffset.transform.forward * 100f);
    }
#endif
}