using UnityEngine;

public class TVController : MonoBehaviour
{
    [SerializeField] private Renderer _screenRenderer;
    [SerializeField] private Material _renderTextureMaterial;

    public void Fix()
    {
        if (_screenRenderer != null && _renderTextureMaterial != null)
        {
            _screenRenderer.material = _renderTextureMaterial;
        }
    }
}