Shader "Hovl/Particles/Blend_CenterGlow"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_Flow("Flow", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
		_DistortionSpeedXYPowerZ("Distortion Speed XY Power Z", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 2
		_Color("Color", Color) = (0.5,0.5,0.5,1)
		_Opacity("Opacity", Range( 0 , 3)) = 1
		[Toggle]_Usecenterglow("Use center glow?", Float) = 0
		[MaterialToggle] _Usedepth ("Use depth?", Float ) = 0
        _Depthpower ("Depth power", Float ) = 1
		[Enum(Cull Off,0, Cull Front,1, Cull Back,2)] _CullMode("Culling", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		
		//_MainTex、_Noise、_Flow、_Mask 是四个纹理（贴图）属性，用于渲染的各种效果。
		//_SpeedMainTexUVNoiseZW 和 _DistortionSpeedXYPowerZ 是向量类型的属性，用于控制纹理的动画速度和流动变形的速度。
		//_Emission 控制发光强度。
		//_Color 设置基础颜色。
		//_Opacity 控制透明度（透明度范围为0到3）。
		//_Usecenterglow 是一个切换开关，控制是否启用“中心发光”效果。
		//_Usedepth 控制是否启用软粒子深度处理。
		//_CullMode 控制面剔除模式（正面剔除、背面剔除或不剔除）。
		//_Depthpower 用于控制深度衰减强度。
	}

	Category 
	{
		SubShader  //SubShader 定义了该着色器的渲染过程。多个SubShader可以根据不同硬件进行适配。
		{
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			//Tags: 定义了渲染队列为透明对象，并且不受投影器影响。RenderType 被设置为透明，PreviewType 是平面，用于编辑器中的预览。
			
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend SrcAlpha OneMinusSrcAlpha: 定义了透明度混合模式（源颜色使用SrcAlpha，目标颜色使用OneMinusSrcAlpha，即标准的Alpha混合模式）。
			//最终颜色=源颜色×源透明值+目标颜色×(1−源透明值)
			//这意味着源颜色的透明度会影响最终的颜色混合结果。具体来说，源颜色的透明度越高，目标颜色的贡献就越大；反之，源颜色的透明度越低，源颜色的贡献就越大‌

			ColorMask RGB			 // 表示只写入颜色的RGB通道，不写入Alpha通道。
			Cull[_CullMode]				//控制面剔除模式，_CullMode 可以通过材质的设置进行调整，决定剔除正面、背面或不剔除。
			Lighting Off			//禁用光照，表示此着色器不受光照影响。
			ZWrite Off					// 禁用深度写入，意味着粒子不会更新深度缓冲。
			ZTest LEqual				//使用深度测试，允许当前像素的深度值小于或等于已有的深度值通过测试。
			//ZTest LEqual（深度小于等于当前缓存则通过） 注意，ZTest Off等同于ZTest Always，关闭深度测试等于完全通过
			//方法一：让绿色的对象不被前面的立方体遮挡，一种方式是关闭前面的蓝色立方体深度写入：
			//方法二：另一种方式是让绿色强制通过深度测试：
			//那么如果红色的也开了ZTest Always会怎么样？
			//再看一下Greater相关的部分有什么作用，这次我们其他的都是用默认的渲染状态，绿色的立方体shader中ZTest设置为Greater：
			
			//之前一篇渲染队列的例子中，虽然渲染的顺序反了过来，但是物体之间的遮挡关系仍然是正确的，这就是z-buffer的功劳，
			//不论我们的渲染顺序怎样，遮挡关系仍然能够保持正确。而我们对z-buffer的调用就是通过ZTest和ZWrite来实现的。
			
			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};
				//appdata_t 是输入数据结构，包含了顶点位置、颜色和纹理坐标。
				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO	
				};		
				//是输出数据结构，包含了顶点位置、颜色、纹理坐标等。
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform float _Usecenterglow;
				uniform float4 _SpeedMainTexUVNoiseZW;
				uniform sampler2D _Flow;
				uniform float4 _DistortionSpeedXYPowerZ;
				uniform float4 _Flow_ST;
				uniform sampler2D _Mask;
				uniform float4 _Mask_ST;
				uniform sampler2D _Noise;
				uniform float4 _Noise_ST;
				uniform float4 _Color;
				uniform float _Emission;
				uniform float _Opacity;
				uniform fixed _Usedepth;
				uniform float _Depthpower;

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					
					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
				// UNITY_SETUP_INSTANCE_ID(v) 设置实例ID（在处理实例化渲染时使用）。
				// UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o) 用于支持立体渲染（立体视图模式）。
				// UnityObjectToClipPos(v.vertex) 将物体空间的顶点坐标转换到裁剪空间。
				// #ifdef SOFTPARTICLES_ON 判断是否启用了软粒子效果，计算粒子的深度信息。
				// 最后，返回包含顶点位置、颜色、纹理坐标等数据的v2f结构。

				fixed4 frag ( v2f i  ) : SV_Target
				{
					float lp = 1;
					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate ((sceneZ-partZ) / _Depthpower);
						lp *= lerp(1, fade, _Usedepth);
						i.color.a *= lp;
					#endif

					// 片段着色器的作用是计算每个像素的颜色值，返回给GPU进行渲染。这里使用了软粒子效果：
					// SAMPLE_DEPTH_TEXTURE_PROJ 获取当前像素的深度值，并通过LinearEyeDepth转换为视距深度。
					// fade 根据粒子与场景的深度差计算一个衰减因子（控制软粒子效果的透明度）。
					
					float2 appendResult21 = (float2(_SpeedMainTexUVNoiseZW.x , _SpeedMainTexUVNoiseZW.y));
					float2 uv0_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					float2 panner107 = ( 1.0 * _Time.y * appendResult21 + uv0_MainTex);
					float2 appendResult100 = (float2(_DistortionSpeedXYPowerZ.x , _DistortionSpeedXYPowerZ.y));
					float3 uv0_Flow = i.texcoord.xyz;
					uv0_Flow.xy = i.texcoord.xy * _Flow_ST.xy + _Flow_ST.zw;
					float2 panner110 = ( 1.0 * _Time.y * appendResult100 + (uv0_Flow).xy);
					float2 uv_Mask = i.texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
					float4 tex2DNode33 = tex2D( _Mask, uv_Mask );
					float Flowpower102 = _DistortionSpeedXYPowerZ.z;
					float4 tex2DNode13 = tex2D( _MainTex, ( panner107 - ( (( tex2D( _Flow, panner110 ) * tex2DNode33 )).rg * Flowpower102 ) ) );
					float2 appendResult22 = (float2(_SpeedMainTexUVNoiseZW.z , _SpeedMainTexUVNoiseZW.w));
					float2 uv0_Noise = i.texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
					float2 panner108 = ( 1.0 * _Time.y * appendResult22 + uv0_Noise);
					float4 tex2DNode14 = tex2D( _Noise, panner108 );
					float3 temp_output_78_0 = (( tex2DNode13 * tex2DNode14 * _Color * i.color )).rgb;
					float4 temp_cast_0 = ((1.0 + (uv0_Flow.z - 0.0) * (0.0 - 1.0) / (1.0 - 0.0))).xxxx;
					float4 clampResult38 = tex2DNode33 - temp_cast_0;
					float4 clampResult40 = clamp( ( tex2DNode33 * clampResult38 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
					float4 appendResult87 = (float4(( lerp(temp_output_78_0,( temp_output_78_0 * (clampResult40).rgb ),_Usecenterglow) * _Emission ) , ( tex2DNode13.a * tex2DNode14.a * _Color.a * i.color.a * _Opacity )));
					fixed4 col = appendResult87;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}	
}