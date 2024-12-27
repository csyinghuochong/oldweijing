
Shader "E3DEffect/AirDistort/AutoNoise-Mask-Fre"
{
	 Properties
    {
        _DistortMap("DistortMap", 2D) = "white" {}
        _Speed("Speed", Range(0, 1)) = 0.1
        _Distrot("Distrot", Range(0, 0.1)) = 0.1
        _DistortOffset("DistortOffset", Range(-2, 2)) = -0.8
        _MaskMap("MaskMap", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 5)) = 2
        _Tiling("Tiling", Range(0, 5)) = 0
        _DisFrePower("Dis-Fre-Power", Range(0, 30)) = 6.318185
        _DisFreScale("Dis-Fre-Scale", Range(0, 2)) = 1.430902
        [Toggle]_DisFreinvert("Dis-Fre-invert", Float) = 1
        [HDR]_FreColor("Fre-Color", Color) = (1,1,1,1)
        _ColorFrePower("Color-Fre-Power", Range(0, 30)) = 6.318185
        _ColorFreScale("Color-Fre-Scale", Range(0, 2)) = 1.430902
        [Toggle]_ColorFreinvert("Color-Fre-invert", Float) = 1
        [HideInInspector] _texcoord("", 2D) = "white" {}
        [HideInInspector] __dirty("", Int) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Overlay+0" "IsEmissive" = "true" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            ZWrite Off
            ZTest Always
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Properties
            TEXTURE2D(_DistortMap);
            SAMPLER(sampler_DistortMap);
            TEXTURE2D(_MaskMap);
            SAMPLER(sampler_MaskMap);
            TEXTURE2D(_GrabTexture);
            SAMPLER(sampler_GrabTexture);
            float4 _FreColor;
            float _ColorFreinvert;
            float _ColorFreScale;
            float _ColorFrePower;
            float _Speed;
            float _DistortOffset;
            float _Alpha;
            float _Distrot;
            float _Tiling;
            float _DisFreinvert;
            float _DisFreScale;
            float _DisFrePower;

            // Vertex structure
            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };

            // Fragment output structure
            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };

            // Vertex Shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;
                o.uv = v.uv * _Tiling;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.worldNormal);
                o.screenPos = ComputeScreenPos(o.position);
                return o;
            }

            // Fragment Shader
            half4 frag(Varyings i) : SV_Target
            {
                // Fresnel calculations
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                float fresnelNdotV = dot(i.worldNormal, worldViewDir);
                float fresnelColor = pow(1.0 - fresnelNdotV, _ColorFrePower) * _ColorFreScale;

                // Distort map sampling with time-based animation
                float2 uvDistort = i.uv + (_Speed * _Time.y * float2(0.24, 0.24));
                float4 distortTex = tex2D(_DistortMap, uvDistort);

                // Mask map sampling
                float4 maskTex = tex2D(_MaskMap, i.uv * _Tiling + distortTex.xy * _DistortOffset);
                float4 maskResult = maskTex * maskTex.a * i.color.a * _Alpha;

                // Screen color sampling
                float2 uvScreen = lerp(i.uv, distortTex.xy * _DistortOffset, saturate(maskResult.r * _Distrot));
                float4 screenColor = tex2Dproj(_GrabTexture, float4(uvScreen, 0.0, 1.0));

                // Final emission calculation
                half3 finalColor = _FreColor.rgb * lerp(saturate(1.0 - fresnelColor), saturate(fresnelColor), _ColorFreinvert);
                finalColor += screenColor.rgb;

                // Final output
                half4 outputColor = half4(finalColor, maskResult.r);
                return outputColor;
            }

            ENDHLSL
        }
    }

    Fallback "Unlit/Color"
}