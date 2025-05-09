using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TintRenderFeature : ScriptableRendererFeature
{
    private TintPass _pass;
    
    public override void Create()
    {
        
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_pass);
    }

    public class TintPass : ScriptableRenderPass
    {
        private Material _mat;
        private RenderTargetIdentifier _src, _tint;
        int tintId = Shader.PropertyToID("_Temp");

        public TintPass()
        {
            _mat = CoreUtils.CreateEngineMaterial("CustomPost/ScreenTint");
            renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            _src = renderingData.cameraData.renderer.cameraColorTargetHandle;
            cmd.GetTemporaryRT(tintId, desc, FilterMode.Bilinear);
            _tint = new RenderTargetIdentifier(tintId);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var commandBuffer = CommandBufferPool.Get("TintRenderFeature");
            VolumeStack stack = VolumeManager.instance.stack;
            CustomPostScreenTint tintData = stack.GetComponent<CustomPostScreenTint>();
            if (tintData.IsActive())
            {
                _mat.SetColor("_OverlayColor", (Color)tintData.tintColor);
                _mat.SetFloat("_Intensity", (float)tintData.tintIntensity);
                
                Blit(commandBuffer, _src, _tint, _mat, 0);
                Blit(commandBuffer, _tint, _src);
            }
            
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tintId);
        }
    }
}
