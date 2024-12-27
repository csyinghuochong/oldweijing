
Shader "E3DEffect/C1/AirWave" {
 Properties
    {
        _NoiseTex("Noise Texture (RG)", 2D) = "white" {}
        _MainTex("Alpha (A)", 2D) = "white" {}
        _HeatTime("Heat Time", range(0, 1.5)) = 1
        _HeatForce("Heat Force", range(0, 6)) = 0.1
    }

    SubShader
    {
        Tags { "Queue" = "Overlay+1" "RenderType" = "Transparent" }

        Pass
        {
            Name "BASE"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite Off
            ZTest LEqual
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Properties
            TEXTURE2D(_NoiseTex);
            TEXTURE2D(_MainTex);
            float _HeatForce;
            float _HeatTime;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;

            // Input structure
            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 uvgrab : TEXCOORD0;
                float2 uvmain : TEXCOORD1;
            };

            // Vertex shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex.xyz);

                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uvgrab.xy = (float2(o.position.x, o.position.y * scale) + o.position.w) * 0.5;
                o.uvgrab.zw = o.position.zw;
                o.uvmain = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Fragment shader
            half4 frag(Varyings i) : SV_Target
            {
                // Noise effect based on time and heat
                half4 offsetColor1 = tex2D(_NoiseTex, i.uvmain + _Time.xz * _HeatTime);
                half4 offsetColor2 = tex2D(_NoiseTex, i.uvmain - _Time.yx * _HeatTime);

                // Modify UVs based on the noise and heat force
                i.uvgrab.x += ((offsetColor1.r + offsetColor1.r) - 1) * _HeatForce;
                i.uvgrab.y += ((offsetColor2.g + offsetColor2.g) - 1) * _HeatForce;

                // Sample the "Grabbed" texture (substitute for GrabPass)
                half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));

                // Always set alpha to 1
                col.a = 1.0f;

                // Tint the color from _MainTex (forcing RGB to 1)
                half4 tint = tex2D(_MainTex, i.uvmain);
                tint.rgb = 1.0f;

                return col * tint;
            }

            ENDHLSL
        }
    }

    SubShader
    {
        Tags { "LightMode" = "UniversalForward" }

        Pass
        {
            Name "BASE"
            Blend DstColor Zero
            SetTexture[_MainTex]
            {
                combine primary
            }
        }
    }

    Fallback "Unlit/Color"
}