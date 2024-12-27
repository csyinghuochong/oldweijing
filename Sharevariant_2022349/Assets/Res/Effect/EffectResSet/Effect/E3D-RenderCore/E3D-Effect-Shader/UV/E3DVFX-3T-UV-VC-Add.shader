
Shader "E3D/Effect/E3DVFX-3T-UV-VC-Add"
{
	 Properties
    {
        [HDR]_BaseColor("BaseColor", Color) = (1, 1, 1, 0)
        _BaseMap("BaseMap", 2D) = "white" {}
        _MaskMap("MaskMap", 2D) = "white" {}
        _DetailMap("DetailMap", 2D) = "white" {}
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
            Cull Off
            Blend One One

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariables.hlsl"

            // Properties
            TEXTURE2D(_BaseMap);
            TEXTURE2D(_MaskMap);
            TEXTURE2D(_DetailMap);
            float4 _BaseColor;

            // Vertex Structure
            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Fragment Output Structure
            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Vertex Shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader
            half4 frag(Varyings i) : SV_Target
            {
                // Sample the Base, Mask, and Detail textures
                float4 baseTexColor = tex2D(_BaseMap, i.uv);
                float4 maskTexColor = tex2D(_MaskMap, i.uv);
                float4 detailTexColor = tex2D(_DetailMap, i.uv);

                // Calculate emission color
                half3 emission = (baseTexColor.rgb * maskTexColor.rgb * detailTexColor.rgb) * _BaseColor.rgb * i.color.rgb;

                // Set alpha to 1 as in the original shader
                return half4(emission, 1.0);
            }

            ENDHLSL
        }
    }

    Fallback "Unlit/Color"
}