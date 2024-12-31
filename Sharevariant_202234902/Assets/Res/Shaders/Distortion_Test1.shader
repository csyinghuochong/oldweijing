Shader "Custom/Distortion_Test1"
{

	Properties
	{
		_Diffuse ( "Diffuse", Color ) = (1,1,1,1)
	}

	Category 
	{
		SubShader
		{
			Tags { }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			Lighting Off 
			ZWrite Off
			
			Pass {		
				
				Tags { "LightMode"="ForwardBase" }
				
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "Lighting.cginc"

				fixed4 _Diffuse;
				
				struct a2v 
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;				
				};

				struct v2f 
				{
					float4 pos : SV_POSITION;
					fixed3 color : COLOR;
					float4 texcoord : TEXCOORD1;
					float2 texcoord2 : TEXCOORD2;
				};			

				v2f vert ( a2v v  )
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);

					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

					fixed3 worldNormal = normalize( mul( v.normal, (float3x3)unity_WorldToObject ) );

					fixed3 worldLight = normalize( _WorldSpaceCameraPos.xyz );

					fixed3 diffuse = _LightColor0.rgb * _Diffuse.rbg * saturate( dot (worldNormal, worldLight) );

					o.color = ambient + diffuse;
					
					return o;
					
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					return fixed4(i.color,1.0);
				}
				ENDCG 
			}
		}	
	}	
}
