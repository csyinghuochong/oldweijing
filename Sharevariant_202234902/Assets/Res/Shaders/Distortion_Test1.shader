Shader "Custom/Distortion_Test1"
{

     Properties
    {
       
         _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Distortion ("Distortion", Range(0, 1)) = 0.0f
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float _Distortion;
            CBUFFER_END

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float4 texcoord : TEXCOORD1;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                output.position = TransformObjectToHClip(input.vertex);

                // Assuming we want to use world space normal for some calculations
                output.normalWS = TransformObjectToWorldNormal(input.normal);

                // Pass through UVs
                output.texcoord = input.texcoord;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Example distortion effect: simply blending the base color with a distorted version
                // Here you would typically apply your distortion logic
                half4 color = _BaseColor;

                // Example distortion: simple modulation based on _Distortion value
                // In a real distortion effect, you would use input.texcoord and possibly other values
                // to create a more complex distortion pattern
                color.rgb *= lerp(1.0, sin(_Time.y * 10.0 + input.texcoord.x * 100.0) * 0.5 + 0.5, _Distortion);

                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}


//{
//	Properties
//	{
//		_Diffuse ( "Diffuse", Color ) = (1,1,1,1)
//	}
//
//	Category 
//	{
//		SubShader
//		{
//			Tags { }
//			Blend SrcAlpha OneMinusSrcAlpha
//			Cull Off
//			Lighting Off 
//			ZWrite Off
//			
//			Pass {		
//				
//				Tags { "LightMode"="ForwardBase" }
//				
//				CGPROGRAM
//				#pragma vertex vert
//				#pragma fragment frag
//
//				#include "Lighting.cginc"
//
//				fixed4 _Diffuse;
//				
//				struct a2v 
//				{
//					float4 vertex : POSITION;
//					float3 normal : NORMAL;
//					fixed4 color : COLOR;
//					float4 texcoord : TEXCOORD0;				
//				};
//
//				struct v2f 
//				{
//					float4 pos : SV_POSITION;
//					fixed3 color : COLOR;
//					float4 texcoord : TEXCOORD1;
//					float2 texcoord2 : TEXCOORD2;
//				};			
//
//				v2f vert ( a2v v  )
//				{
//					v2f o;
//					o.pos = UnityObjectToClipPos(v.vertex);
//
//					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
//
//					fixed3 worldNormal = normalize( mul( v.normal, (float3x3)unity_WorldToObject ) );
//
//					fixed3 worldLight = normalize( _WorldSpaceCameraPos.xyz );
//
//					fixed3 diffuse = _LightColor0.rgb * _Diffuse.rbg * saturate( dot (worldNormal, worldLight) );
//
//					o.color = ambient + diffuse;
//					
//					return o;
//					
//				}
//
//				fixed4 frag ( v2f i  ) : SV_Target
//				{
//					return fixed4(i.color,1.0);
//				}
//				ENDCG 
//			}
//		}	
//	}	
//}
