using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PSX
{
    public class CRTRenderFeature : ScriptableRendererFeature
    {
        private const string ShaderPath = "PostEffect/CRTShader";
        
        CRTPass _crtPass;
        private Shader _shader;
        private Material _material;

        public override void Create()
        {
            _shader = Shader.Find(ShaderPath);
            _material = new Material(_shader);
            _crtPass = new CRTPass(_material);
            _crtPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_crtPass);
        }
    }


    public class CRTPass : ScriptableRenderPass
    {
        private static readonly string shaderPath = "PostEffect/CRTShader";
        static readonly string k_RenderTag = "Render CRT Effects";
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        static readonly int TempTargetId = Shader.PropertyToID("_TempTargetCRT");

        static readonly int ScanLinesWeight = Shader.PropertyToID("_ScanlinesWeight");
        static readonly int NoiseWeight = Shader.PropertyToID("_NoiseWeight");
        
        static readonly int ScreenBendX = Shader.PropertyToID("_ScreenBendX");
        static readonly int ScreenBendY = Shader.PropertyToID("_ScreenBendY");
        static readonly int VignetteAmount = Shader.PropertyToID("_VignetteAmount");
        static readonly int VignetteSize = Shader.PropertyToID("_VignetteSize");
        static readonly int VignetteRounding = Shader.PropertyToID("_VignetteRounding");
        static readonly int VignetteSmoothing = Shader.PropertyToID("_VignetteSmoothing");

        static readonly int ScanLinesDensity = Shader.PropertyToID("_ScanLinesDensity");
        static readonly int ScanLinesSpeed = Shader.PropertyToID("_ScanLinesSpeed");
        static readonly int NoiseAmount = Shader.PropertyToID("_NoiseAmount");

        static readonly int ChromaticRed = Shader.PropertyToID("_ChromaticRed");
        static readonly int ChromaticGreen = Shader.PropertyToID("_ChromaticGreen");
        static readonly int ChromaticBlue = Shader.PropertyToID("_ChromaticBlue");
        
        static readonly int GrilleOpacity = Shader.PropertyToID("_GrilleOpacity");
        static readonly int GrilleCounterOpacity = Shader.PropertyToID("_GrilleCounterOpacity");
        static readonly int GrilleResolution = Shader.PropertyToID("_GrilleResolution");
        static readonly int GrilleCounterResolution = Shader.PropertyToID("_GrilleCounterResolution");
        static readonly int GrilleBrightness = Shader.PropertyToID("_GrilleBrightness");
        static readonly int GrilleUvRotation = Shader.PropertyToID("_GrilleUvRotation");
        static readonly int GrilleUvMidPoint = Shader.PropertyToID("_GrilleUvMidPoint");
        static readonly int GrilleShift = Shader.PropertyToID("_GrilleShift");

        private Crt m_Crt;
        private Material crtMaterial;
        private RenderTextureDescriptor _crtTextureDescriptor;
        private RTHandle _crtTextureHandle;

        public CRTPass(Material material) 
        {
            crtMaterial = material;
            _crtTextureDescriptor = new RenderTextureDescriptor(Screen.width,
                Screen.height, RenderTextureFormat.Default, 0);
        }

        public override void Configure(CommandBuffer cmd,
            RenderTextureDescriptor cameraTextureDescriptor)
        {
            _crtTextureDescriptor.width = cameraTextureDescriptor.width;
            _crtTextureDescriptor.height = cameraTextureDescriptor.height;

            RenderingUtils.ReAllocateIfNeeded(ref _crtTextureHandle, _crtTextureDescriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (this.crtMaterial == null)
            {
                Debug.LogError("Material not created.");
                return;
            }

            if (!renderingData.cameraData.postProcessEnabled) return;
            
            UpdateCrtSettings();

            var cmd = CommandBufferPool.Get(k_RenderTag);
            ref var cameraData = ref renderingData.cameraData;
            cameraData.camera.depthTextureMode = cameraData.camera.depthTextureMode | DepthTextureMode.Depth;
            var source = cameraData.renderer.cameraColorTargetHandle;

            cmd.Blit(source, _crtTextureHandle);
            cmd.Blit(_crtTextureHandle, source, this.crtMaterial, 0);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        private void UpdateCrtSettings()
        {
            if (crtMaterial == null) return;
            var stack = VolumeManager.instance.stack;

            this.m_Crt = stack.GetComponent<Crt>();
            if (this.m_Crt == null) return;
            if (!this.m_Crt.IsActive()) return;
            
            this.crtMaterial.SetFloat(ScanLinesWeight, this.m_Crt.scanlinesWeight.value);
            this.crtMaterial.SetFloat(NoiseWeight, this.m_Crt.noiseWeight.value);
            
            this.crtMaterial.SetFloat(ScreenBendX, this.m_Crt.screenBendX.value);
            this.crtMaterial.SetFloat(ScreenBendY, this.m_Crt.screenBendY.value);
            this.crtMaterial.SetFloat(VignetteAmount, this.m_Crt.vignetteAmount.value);
            this.crtMaterial.SetFloat(VignetteSize, this.m_Crt.vignetteSize.value);
            this.crtMaterial.SetFloat(VignetteRounding, this.m_Crt.vignetteRounding.value);
            this.crtMaterial.SetFloat(VignetteSmoothing, this.m_Crt.vignetteSmoothing.value);

            this.crtMaterial.SetFloat(ScanLinesDensity, this.m_Crt.scanlinesDensity.value);
            this.crtMaterial.SetFloat(ScanLinesSpeed, this.m_Crt.scanlinesSpeed.value);
            this.crtMaterial.SetFloat(NoiseAmount, this.m_Crt.noiseAmount.value);

            this.crtMaterial.SetVector(ChromaticRed, this.m_Crt.chromaticRed.value);
            this.crtMaterial.SetVector(ChromaticGreen, this.m_Crt.chromaticGreen.value);
            this.crtMaterial.SetVector(ChromaticBlue, this.m_Crt.chromaticBlue.value);

            this.crtMaterial.SetFloat(GrilleOpacity, this.m_Crt.grilleOpacity.value);
            this.crtMaterial.SetFloat(GrilleCounterOpacity, this.m_Crt.grilleCounterOpacity.value);
            this.crtMaterial.SetFloat(GrilleResolution, this.m_Crt.grilleResolution.value);
            this.crtMaterial.SetFloat(GrilleCounterResolution, this.m_Crt.grilleCounterResolution.value);
            this.crtMaterial.SetFloat(GrilleBrightness, this.m_Crt.grilleBrightness.value);
            this.crtMaterial.SetFloat(GrilleUvRotation, this.m_Crt.grilleUvRotation.value);
            this.crtMaterial.SetFloat(GrilleUvMidPoint, this.m_Crt.grilleUvMidPoint.value);
            this.crtMaterial.SetVector(GrilleShift, this.m_Crt.grilleShift.value);
        }
    }
}