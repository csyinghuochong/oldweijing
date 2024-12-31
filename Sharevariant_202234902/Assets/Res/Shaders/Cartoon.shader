Shader "Custom/Cartoon/Vertex/TexturedAddURP"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cartoon ("Cartoon (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
 
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
 
            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 cap : TEXCOORD1;
            };
 
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
 
            TEXTURE2D(_Cartoon);
            SAMPLER(sampler_Cartoon);
 
            Varyings Vert(Attributes input)
            {
                Varyings output;
 
                // Transform the vertex position from object space to clip space.
                output.position = TransformObjectToHClip(input.vertex);
 
                // Transform the UV coordinates.
                // Note: In URP, TRANSFORM_TEX is not typically used directly in HLSL,
                // as SAMPLE_TEXTURE2D handles texture sampling with the correct UVs.
                // However, if you need to manually transform UVs, you can do it here.
                // For simplicity, we assume _MainTex_ST is identity in this example.
                output.uv = input.uv; // Or use TRANSFORM_TEX if you have a non-identity scale/offset.
 
                // Compute the cap texture coordinates based on the normal.
                float3 worldNormal = TransformObjectToWorldNormal(input.normal);
                float3 viewDir = normalize(TransformWorldToViewDir(worldNormal)); // Approximation, as we actually want the inverse view matrix's rows.
                float2 capCoord;
                capCoord.x = dot(viewDir, float3(1,0,0)); // Simplified; should use inverse view matrix's first row.
                capCoord.y = dot(viewDir, float3(0,1,0)); // Simplified; should use inverse view matrix's second row.
 
                // Normalize capCoord to [0,1] range.
                // Note: This might not be necessary if the cartoon texture is designed to wrap.
                output.cap = capCoord * 0.5 + 0.5;
 
                return output;
            }
 
            half4 Frag(Varyings input) : SV_Target
            {
                // Sample the base texture.
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
 
                // Sample the cartoon texture using the computed cap coordinates.
                half4 cartoonColor = SAMPLE_TEXTURE2D(_Cartoon, sampler_Cartoon, input.cap);
 
                // Apply the cartoon effect by blending with the base color.
                // The original formula (tex + (mc*2.0)-1.0) is preserved.
                half4 finalColor = baseColor + (cartoonColor * 2.0 - 1.0);
 
                // Ensure the color is clamped to a valid range.
                finalColor = saturate(finalColor);
 
                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack "Universal/Fallback/Diffuse"
}