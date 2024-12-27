Shader "Hovl/Particles/Distortion"
{
	 Properties
    {
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Distortionpower("Distortion Power", Float) = 0.05
        _InvFade("Soft Particles Factor", Range(0.01, 3.0)) = 1.0
    }

	 
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" "PreviewType"="Plane" }

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma target 4.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphUV.hlsl"

            // Properties
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            float _Distortionpower;
            float _InvFade;
            TEXTURE2D(_GrabTexture);
            SAMPLER(sampler_GrabTexture);
            float4 _GrabTexture_TexelSize;

            // Input structure
            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Output structure
            struct Varyings
            {
                float4 position : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 grabTexCoord : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.position = TransformObjectToHClip(v.vertex.xyz);
                o.color = v.color;

                o.uv = v.uv;
                o.grabTexCoord = float4(v.uv, 0.0, 1.0); // Grab texture coordinate

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Sample normal map for distortion
                half3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
                half2 distortion = normal.rg;

                // Calculate the screen space distortion
                float2 screenPos = i.grabTexCoord.xy * i.grabTexCoord.w;
                screenPos = screenPos + distortion * _Distortionpower;

                // Sample grab texture with distortion applied
                half4 col = tex2Dproj(_GrabTexture, float4(screenPos, 0.0, 1.0));

                // Apply alpha fade for soft particles (depth-based fade)
                #ifdef SOFTPARTICLES_ON
                // Calculate depth fade
                float sceneDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.grabTexCoord);
                float particleDepth = i.grabTexCoord.z;
                float fade = saturate(_InvFade * (sceneDepth - particleDepth));
                col.a *= fade;
                #endif

                return col;
            }
            ENDHLSL
        }
    }

    // Fallback shader if URP is not available
    Fallback "Unlit/Color"
}
