
Shader "E3D/EffectEN/ADD_Top"
{
	 Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        [HDR]_MainColor("MainColor", Color) = (1, 1, 1, 1)
        [HideInInspector] _texcoord("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Overlay+0" }
        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest Always
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariables.hlsl"

            // Properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainColor;

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
                // Sample the main texture
                float4 texColor = tex2D(_MainTex, i.uv);
                // Multiply with vertex color and main color
                texColor.rgb *= i.color.rgb * _MainColor.rgb;
                texColor.a = 1.0; // Set alpha to 1 as per the original shader

                return texColor;
            }

            ENDHLSL
        }
    }

    Fallback "Unlit/Color"
}