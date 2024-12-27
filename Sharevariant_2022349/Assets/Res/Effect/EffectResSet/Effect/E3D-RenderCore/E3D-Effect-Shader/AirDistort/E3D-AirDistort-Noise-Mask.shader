
Shader "E3DEffect/AirDistort/Noise-Mask"
{
 Properties
    {
        _DistortMap("DistortMap", 2D) = "white" {}
        _Distrot("Distrot", Range(0, 0.1)) = 0.255728
        _DistortOffset("DistortOffset", Range(-2, 2)) = -0.8
        _MaskMap("MaskMap", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 5)) = 2
        [HideInInspector] _texcoord("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Overlay+0" "IsEmissive" = "true" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite Off
            ZTest Always
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Properties
            TEXTURE2D(_DistortMap);
            TEXTURE2D(_MaskMap);
            TEXTURE2D(_GrabTexture);
            float4 _DistortMap_ST;
            float _DistortOffset;
            float _Alpha;
            float _Distrot;

            // Vertex Structure
            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            // Fragment Output Structure
            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            // Vertex Shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(v.vertex);
                return o;
            }

            // Fragment Shader
            half4 frag(Varyings i) : SV_Target
            {
                // Sample the DistortMap and MaskMap textures
                float2 uv_DistortMap = i.uv * _DistortMap_ST.xy + _DistortMap_ST.zw;
                float4 distortMapColor = tex2D(_DistortMap, uv_DistortMap);
                float2 distortOffset = distortMapColor.rg * _DistortOffset;

                // Sample the MaskMap texture
                float2 uv_MaskMap = i.uv * _MaskMap_ST.xy + _MaskMap_ST.zw;
                float4 maskMapColor = tex2D(_MaskMap, uv_MaskMap);

                // Calculate the final distortion based on MaskMap and DistortMap
                float4 clampResult = clamp(maskMapColor * maskMapColor.a * i.color.a * _Alpha, 0.0, 1.0);
                float2 finalUV = lerp(i.screenPos.xy, distortOffset, saturate(clampResult.r * _Distrot));

                // Sample the Grabbed Screen Texture (grabbed texture from screen)
                float4 screenColor = tex2D(_GrabTexture, finalUV);

                // Set the final color and alpha (Emission and Alpha)
                half4 outputColor;
                outputColor.rgb = screenColor.rgb;
                outputColor.a = clampResult.r;

                return outputColor;
            }

            ENDHLSL
        }
    }

    Fallback "Unlit/Color"
}