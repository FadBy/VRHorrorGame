Shader "CustomEffects/Blur"
{
    HLSLINCLUDE
    
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    CBUFFER_START(UnityPerMaterial)
        float4 _BlitTexture_TexelSize;
        float4 _Color;
        float _Offset;
        float _Frequency;
        float _StripeSpeed;
    CBUFFER_END

        float3 getOffsetTexel(float2 texcoord, float2 offset)
        {
            return SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, texcoord + offset);
        }

        float CheckerBoard(float y, float frequency, float offset)
        {
            
            return frac(y * frequency + offset) > 0.5f;
        }
    
        float4 Blur(Varyings input) : SV_Target
        {
            float3 color = (
                float3(1, 0, 0) * getOffsetTexel(input.texcoord, float2(_Offset, 0)) +
                float3(0, 0, 1)  * getOffsetTexel(input.texcoord, float2(-_Offset, 0)) +
                getOffsetTexel(input.texcoord, float2(0, _Offset)) +
                getOffsetTexel(input.texcoord, float2(0, -_Offset))
                ) / 4;

            
            return float4(color, 1) * _Color * CheckerBoard(input.texcoord.y, _Frequency, _Time * _StripeSpeed);
        }
    
    ENDHLSL
    
    SubShader
    {
        Tags { "RenderType"="Opaque""RenderPipeline" = "UniversalPipeline"}
        ZWrite Off Cull Off
        Pass
        {
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment Blur
            
            ENDHLSL
        }
    }
}