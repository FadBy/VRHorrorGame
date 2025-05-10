using System;
using TMPro;
using UnityEngine;
using Zenject;

public class TVController : MonoBehaviour
{
    [SerializeField] private Renderer _screenRenderer;
    [SerializeField] private Material _renderTextureMaterial;
    [SerializeField] private TextMeshProUGUI _codeDisplay;
    [SerializeField] private AudioSource _audioSource;

    private CodeController _codeController;
    
    [Inject]
    public void Construct(CodeController codeController)
    {
        _codeController = codeController;
    }
    
    private void Start()
    {
        _codeDisplay.text = _codeController.Code.ToString();
    }

    public void Fix()
    {
        _audioSource.Stop();
        if (_screenRenderer != null && _renderTextureMaterial != null)
        {
            _screenRenderer.material = _renderTextureMaterial;
        }
    }
}