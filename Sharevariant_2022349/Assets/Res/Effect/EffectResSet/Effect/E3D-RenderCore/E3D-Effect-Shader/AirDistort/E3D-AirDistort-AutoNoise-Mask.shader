
Shader "E3DEffect/AirDistort/AutoNoise-Mask"
{
	 Properties
    {
        _DistortMap("DistortMap", 2D) = "white" {}
        _Speed("Speed", Range(0, 1)) = 0.1
        _Distrot("Distrot", Range(0, 0.1)) = 0.255728
        _DistortOffset("DistortOffset", Range(-2, 2)) = -0.8
        _MaskMap("MaskMap", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 5)) = 2
        _Tiling("Tiling", Range(0, 5)) = 0
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
            float _Speed;
            float _DistortOffset;
            float _Alpha;
            float _Distrot;
            float _Tiling;

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
                // Sample the DistortMap with time-based panning for animation
                float mulTime26 = _Time.y * _Speed;
                float2 panner32 = float2( mulTime26 * 0.24, mulTime26 * 0.24 );
                float2 uv_TexCoord27 = i.uv * _Tiling + panner32;
                float4 tex2DNode9 = tex2D(_DistortMap, uv_TexCoord27);

                // Another panner for added distortion effect
                float mulTime40 = _Time.y * _Speed;
                float2 panner41 = float2( mulTime40 * -0.24, mulTime40 * 0.1 );
                float2 uv_TexCoord37 = i.uv * _Tiling + panner41;
                float4 tex2DNode34 = tex2D(_DistortMap, uv_TexCoord37);

                // Combine distortions
                float2 appendResult17 = float2(tex2DNode9.r + tex2DNode34.r, tex2DNode9.g + tex2DNode34.r);

                // Sample the MaskMap
                float2 uv_MaskMap = i.uv * _MaskMap_ST.xy + _MaskMap_ST.zw;
                float4 tex2DNode4 = tex2D(_MaskMap, uv_MaskMap);

                // Compute the alpha value from MaskMap
                float4 clampResult15 = clamp(tex2DNode4 * tex2DNode4.a * i.color.a * _Alpha, 0, 1);

                // Calculate the final distorted UVs
                float2 lerpResult2 = lerp(i.screenPos.xy, appendResult17 * _DistortOffset, saturate(clampResult15.r * _Distrot));

                // Sample the screen color based on the distorted UVs
                float4 screenColor1 = tex2D(_GrabTexture, lerpResult2);

                // Output the final emission and alpha values
                half4 outputColor;
                outputColor.rgb = screenColor1.rgb;
                outputColor.a = clampResult15.r;

                return outputColor;
            }

            ENDHLSL
        }
    }

    Fallback "Unlit/Color"
}