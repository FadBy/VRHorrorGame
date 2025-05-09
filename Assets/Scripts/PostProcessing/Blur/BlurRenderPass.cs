using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderPass : ScriptableRenderPass
{
    private static readonly int colorId =
        Shader.PropertyToID("_Color");
    private static readonly int offsetId =
        Shader.PropertyToID("_Offset");
    private static readonly int frequencyId =
        Shader.PropertyToID("_Frequency");
    private static readonly int stripeSpeedId =
        Shader.PropertyToID("_StripeSpeed");
    
    private BlurSettings defaultSettings;
    private Material material;

    private RenderTextureDescriptor blurTextureDescriptor;
    private RTHandle blurTextureHandle;

    public BlurRenderPass(Material material, BlurSettings defaultSettings)
    {
        this.material = material;
        this.defaultSettings = defaultSettings;

        blurTextureDescriptor = new RenderTextureDescriptor(Screen.width,
            Screen.height, RenderTextureFormat.ARGBHalf, 0);
    }

    public override void Configure(CommandBuffer cmd,
        RenderTextureDescriptor cameraTextureDescriptor)
    {
        // Set the blur texture size to be the same as the camera target size.
        blurTextureDescriptor.width = cameraTextureDescriptor.width;
        blurTextureDescriptor.height = cameraTextureDescriptor.height;

        // Check if the descriptor has changed, and reallocate the RTHandle if necessary
        RenderingUtils.ReAllocateIfNeeded(ref blurTextureHandle, blurTextureDescriptor);
    }

    private void UpdateBlurSettings()
    {
        if (material == null) return;

        // Use the Volume settings or the default settings if no Volume is set.
        var volumeComponent =
            VolumeManager.instance.stack.GetComponent<MyCrt>();
        float offset = volumeComponent.offset.value;
        var color = volumeComponent.colorParameter.value;
        var frequency = volumeComponent.frequency.value;
        var stripeSpeed = volumeComponent.stripeSpeed.value;
        material.SetFloat(offsetId, offset);
        material.SetColor(colorId, color);
        material.SetFloat(frequencyId, frequency);
        material.SetFloat(stripeSpeedId, stripeSpeed);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var volumeComponent = VolumeManager.instance.stack.GetComponent<MyCrt>();

        if (volumeComponent == null || !volumeComponent.active)
        {
            return;
        }
        
        if (!volumeComponent.AnyPropertiesIsOverridden())
        {
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get();

        RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

        UpdateBlurSettings();

        Blit(cmd, cameraTargetHandle, blurTextureHandle, material, 0);
        Blit(cmd, blurTextureHandle, cameraTargetHandle);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }


    public void Dispose()
    {
    #if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            Object.Destroy(material);
        }
        else
        {
            Object.DestroyImmediate(material);
        }
    #else
                Object.Destroy(material);
    #endif

        if (blurTextureHandle != null) blurTextureHandle.Release();
    }
}