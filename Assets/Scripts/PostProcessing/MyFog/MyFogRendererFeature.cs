using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MyFogRendererFeature : ScriptableRendererFeature
{
    // [SerializeField] private BlurSettings settings;
    [SerializeField] private Shader shader;
    private Material material;
    private MyFogRenderPass _myFogRenderPass;

    public override void Create()
    {
        if (shader == null)
        {
            return;
        }
        material = new Material(shader);
        _myFogRenderPass = new MyFogRenderPass(material);
        
        _myFogRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(_myFogRenderPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        _myFogRenderPass.Dispose();
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            Destroy(material);
        }
        else
        {
            DestroyImmediate(material);
        }
#else
                Destroy(material);
#endif
    }
}