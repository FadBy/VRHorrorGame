Shader "CustomEffects/Fog"
{
    HLSLINCLUDE
    
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        TEXTURE2D(_CameraDepthTexture);

        CBUFFER_START(UnityPerMaterial)
            float4 _FogColor;
            float4 _FogDensity;
        CBUFFER_END
        
        float4 Frag(Varyings input) : SV_Target
        {
            _FogDensity = 0.4;
            float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, input.texcoord);
            float linearDepth = Linear01Depth(rawDepth, _ZBufferParams);
            float dist = linearDepth * _ProjectionParams.z;
            float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
            float fogFactor = (_FogDensity / sqrt(log(2))) * max(0.0f, dist - 0);
                fogFactor = exp2(-fogFactor * fogFactor);
            // return fogFactor;
            return float4(lerp(_FogColor, color.rgb, saturate(fogFactor)), 1);
            // return float4(color.rgb, 1);
            // return float4(linearDepth.xxx, 1);
            return float4(frac(linearDepth).xxx, 1);
        }
    
    ENDHLSL
    
    
    SubShader
    {
        Tags { "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline"}
        ZWrite Off Cull Off
        Pass
        {
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment Frag
            
            ENDHLSL
        }
    }
}
