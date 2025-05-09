using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Transform _anchor;
    [SerializeField] private float _radius = 1;
    [SerializeField] private KeyCode _interactKey;
    [SerializeField] private bool _initiallyInteractable = true;

    public UnityEvent OnInteract;
    
    private PlayerMovement _playerMovement;

    public bool Enabled { get; set; }
    
    [Inject]
    public void Construct(PlayerMovement playerMovement)
    {
        Debug.Log("Interactable Injected");
        _playerMovement = playerMovement;
    }

    private void Awake()
    {
        Enabled = _initiallyInteractable;
    }

    private void Update()
    {
        if (!Enabled) return;
        if (IsPlayerInRadius())
        {
            if (DidPlayerInteract())
            {
                OnInteract.Invoke();
            }
        }
    }

    public void Enable()
    {
        Enabled = true;
    }

    public void Disable()
    {
        Enabled = false;
    }

    private bool IsPlayerInRadius()
    {
        var distance = Vector2.Distance(new Vector2(_anchor.position.x, _anchor.position.z),
            new Vector2(_playerMovement.transform.position.x, _playerMovement.transform.position.z));
        return distance <= _radius;
    }

    private bool DidPlayerInteract()
    {
        return Input.GetKeyDown(_interactKey);
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (_anchor == null) return;

        Handles.color = Color.red;
        Handles.DrawWireDisc(_anchor.position, Vector3.up, _radius);
#endif
    }



}