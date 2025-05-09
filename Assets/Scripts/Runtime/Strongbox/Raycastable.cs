using UnityEngine;
using UnityEngine.Events;

public class Raycastable : MonoBehaviour
{
    public UnityEvent OnRaycast;
    
    public void OnRaycastHit()
    {
        OnRaycast.Invoke();
    }
}
