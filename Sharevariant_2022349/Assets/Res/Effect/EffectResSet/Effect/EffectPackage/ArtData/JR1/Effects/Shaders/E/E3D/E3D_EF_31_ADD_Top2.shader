
Shader "E3D/EffectEN/ADD_Top2"
{
	  Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        [HDR]_MainColor("MainColor", Color) = (1,1,1,1)
        [HideInInspector] _texcoord("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Overlay+0" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            ZWrite Off
            ZTest Always
            Cull Off
            Blend One One

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainColor;
            float4 _MainTex_ST;

            // Input structure for vertex data
            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Output structure for fragment shader
            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            
            // Vertex shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            // Fragment shader
            half4 frag(Varyings i) : SV_Target
            {
                // Sample the main texture
                half4 texColor = tex2D(_MainTex, i.uv);

                // Apply the color modulation
                texColor.rgb *= i.color.rgb * _MainColor.rgb;
                texColor.a *= i.color.a * texColor.a * _MainColor.a;

                // Final color output
                return texColor;
            }

            ENDHLSL
        }
    }

    // Fallback shader if URP is not available
    Fallback "Unlit/Color"
}