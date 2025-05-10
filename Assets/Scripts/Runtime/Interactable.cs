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
    [SerializeField] private GameObject _billboardPrefab;
    [SerializeField] private float _billboardYOffset = 0.3f;

    private GameObject _billboard;
    
    public UnityEvent OnInteract;
    
    private PlayerMovement _playerMovement;

    private bool _enabled;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            if (!_enabled)
            {
                _billboard.SetActive(false);
            }
        }
    }

    [Inject]
    public void Construct(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    private void Awake()
    {
        _billboard = Instantiate(_billboardPrefab, _anchor.transform.position + new Vector3(0f, _billboardYOffset, 0f), Quaternion.identity, transform);   
        
        Enabled = _initiallyInteractable;
    }

    private void Update()
    {
        if (!Enabled) return;
        if (_anchor == null) return;
        _billboard.SetActive(IsPlayerInRadius());
        if (IsPlayerInRadius())
        {
            if (DidPlayerInteract())
            {
                Disable();
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