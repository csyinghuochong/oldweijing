Shader "Custom/HighlightDiffuse"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Specular ("Specular", Color) = (0.5,0.5,0.5,1)
        _GlossMap ("Glossiness (A)", 2D) = "white" {}
        _DirectionalLightDir("Directional Light Direction", Vector) = (0, -1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200
 
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
 
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };
 
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_GlossMap);
            SAMPLER(sampler_GlossMap);
 
            float4 _MainTex_ST;
            float _Glossiness;
            float4 _Specular;
            float3 _DirectionalLightDir;
 
           Varyings vert (Attributes input)
            {

                Varyings output;
                // 使用TransformObjectToHClipPos将顶点转换到齐次裁剪空间
                output.positionCS = TransformObjectToHClip(input.vertex);
                // 计算世界空间中的法线
                output.normalWS = TransformObjectToWorldNormal(input.normal);
                // 计算UV坐标
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
 
            half4 frag (Varyings input) : SV_Target
            {
                // Albedo
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
 
                // Normal
                half3 normalWS = normalize(input.normalWS);
 
                // View direction
                half3 viewDirWS = normalize(_WorldSpaceCameraPos - input.positionCS.xyz);
 
                // Light direction (assuming directional light for simplicity)
                half3 lightDirWS =   normalize(_DirectionalLightDir);
 
                // Half vector
                half3 halfVecWS = normalize(lightDirWS + viewDirWS);
 
                // Diffuse term (Lambertian)
                half diffuse = dot(normalWS, lightDirWS) * 0.5 + 0.5;
 
                // Specular term (Blinn-Phong)
                half specularPower = 64.0; // Fixed specular power for simplicity
                half specular = pow(max(0, dot(normalWS, halfVecWS)), specularPower);
                half4 gloss = SAMPLE_TEXTURE2D(_GlossMap, sampler_GlossMap, input.uv) * _Glossiness;
                half4 specularColor = _Specular * gloss.a * specular;
 
                // Combine diffuse and specular
                half4 color = albedo * diffuse + specularColor;
 
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Universal/Diffuse"
} 