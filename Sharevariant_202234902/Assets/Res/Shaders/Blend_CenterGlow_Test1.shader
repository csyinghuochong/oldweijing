Shader "Custom/Blend_CenterGlow_Test1"
{
   Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
	}


	SubShader {
		Tags { "RenderType"="Opaque"  "Queue" = "Geometry+200"}
		//在Unity中，渲染队列（Render Queue）的数值越小，物体越早被渲染‌。具体来说：
		//Background（1000）‌：最早被渲染的物体队列。
		//Geometry（2000）‌：不透明物体的渲染队列，大多数物体都应该使用该队列进行渲染，也是Unity Shader中默认的渲染队列。
		//AlphaTest（2450）‌：有透明通道，需要进行Alpha Test的物体的队列，比Geometry中更有效。
		//Transparent（3000）‌：半透明物体的渲染队列，一般不写深度。
		//Overlay（4000）‌：最后被渲染的物体队列，通常是覆盖效果，如镜头光晕、屏幕贴片等‌
		//因此，‌Geometry队列的数值为2000，意味着它是不透明物体中较早被渲染的队列‌。
		
		//表示在 Geometry 渲染队列的基础上向后移动了300个单位。这个值越大，表示渲染优先级越高。
		//前面两种情况下（只调整渲染队列的顺序），无论是先渲染哪个方块，效果都如下图
		// A物体ZWrite off ，先渲染蓝色再渲染红色。
		// 红色关闭深度写入，没有将像素写入深度缓存中，颜色缓冲区存放的是蓝色的颜色值
		Pass {
			
			Tags {}
			LOD 200
			
			Name "BASE"
			Cull Off
			//ZWrite off 
			//ZTest off   //单独ZTest off， 在任何角度都能看到， 会挡住所有Queue小于他的。。
			CGPROGRAM

			//AB均ZTest off	因为都禁用了深度测试，因此按照渲染顺序来排布，先蓝后红
			//AB均ZWrite off 画面为空 不会写入深度，Colorbuffer里面没有像素值
			////AB均ZTest off + ZWrite off画面为空。
			 
			 #pragma multi_compile_instancing

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;

				//第二步：instancID 加入顶点着色器输入结构 
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float3 cubenormal : TEXCOORD1;
				UNITY_FOG_COORDS(2)

				//第三步：instancID加入顶点着色器输出结构
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			v2f vert (appdata v)
			{
				v2f o;

				 //第四步：instanceid在顶点的相关设置  
                UNITY_SETUP_INSTANCE_ID(v);
                //第五步：传递 instanceid 顶点到片元
                UNITY_TRANSFER_INSTANCE_ID(v, o);


				o.pos = UnityObjectToClipPos (v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				 //第六步：instanceid在片元的相关设置
                UNITY_SETUP_INSTANCE_ID(i);

				fixed4 col = _Color * tex2D(_MainTex, i.texcoord);
				fixed4 cube = texCUBE(_ToonShade, i.cubenormal);
				fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a);
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG			
		}
	} 

	Fallback "VertexLit"
}
