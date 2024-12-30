
Shader "Toon/BasicOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (0.5, 0.5, 0.5, 1)
        [Space]
        _OutlineWidth ("Outline Width", Range(0.0, 1.0)) = 0.15
        _OutlineColor ("Outline Color", Color) = (0.0, 0.0, 0.0, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        //Tags：此标签标记了 Shader 的渲染类型和渲染管线。这里指定了 RenderType 为 Opaque（不透明），并使用 URP 渲染管线（UniversalPipeline）。

        Pass
        {
            Name "MainPass"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            //Pass: 这个 Pass 定义了物体的基本渲染。首先设定名称为 MainPass，并为 UniversalForward 渲染模式，这对应于 URP 的前向渲染模式。
            //‌Less‌：只有当物体的深度值小于当前像素的深度值时，才通过深度测试。
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert

            //#pragma vertex vert: 指定了顶点着色器为 vert。

            #pragma fragment frag

            //#pragma fragment frag: 指定了片段着色器为 frag。

            #pragma multi_compile_instancing

            //#pragma multi_compile_instancing: 启用了实例化支持，用于多实例渲染优化

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //#include "Core.hlsl": 引入 URP 的核心着色器库，提供了常见的渲染函数。

            TEXTURE2D(_MainTex); 
            SAMPLER(sampler_MainTex);

            //纹理和采样器：声明了 _MainTex 纹理，并创建了一个对应的采样器 sampler_MainTex。

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
            CBUFFER_END
            //常量缓冲区：使用 UnityPerMaterial 常量缓冲区来存储每个材质的 _BaseColor。这意味着每个物体可以有不同的基础颜色。

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            //Attributes：包含了从顶点数据传入的属性，包括位置、法线、UV 坐标等。

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            //Varyings：表示从顶点着色器输出到片段着色器的数据结构。包括了 UV 和裁剪空间位置。



            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                //使用 UNITY_SETUP_INSTANCE_ID 和 UNITY_TRANSFER_INSTANCE_ID 处理实例化。

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                //调用 GetVertexPositionInputs 获取顶点位置的相关信息，转换为裁剪空间的位置。

                output.positionCS = vertexInput.positionCS;

                //输出裁剪空间的位置 positionCS 和 UV 坐标。

                output.uv = input.uv;
                return output;
            }
            
            // ‌片元‌是在图元经过光栅化阶段后，被分割成一个个像素大小的基本单位
            // ‌纹理映射‌：从纹理中读取颜色信息，并应用到相应的像素上，实现贴图和细节增强等效果。
            // ‌光照计算‌：执行更详细的光照计算，如计算每个像素上的光照强度和颜色，包括漫反射、镜面反射、环境光等多种光照模型。
            // ‌颜色混合和特殊效果‌：实现各种颜色混合模式，以及应用如模糊、发光、深度测试等后处理效果。
            // ‌输出最终颜色‌：基于上述计算，确定每个像素的最终颜色，并将其发送到渲染管线的下一个阶段（通常是帧缓冲区）‌
            float4 frag(Varyings input) : SV_Target
            {
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return mainTex * _BaseColor;
            }
            //采样 _MainTex 纹理，获取颜色，并将其与 _BaseColor 相乘得到最终的颜色。


            ENDHLSL
        }

        // Outline
        Pass
        {
            Name "Outline"
            Cull Front
            Tags
            {
                "LightMode" = "SRPDefaultUnlit"
            }

            //Cull Front: 通过设置 Cull Front，剔除前面面，确保从外部渲染物体的描边。
            //SRPDefaultUnlit：这是一个无光照的渲染模式，通常用于描边和后期效果。

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            //appdata：输入结构体，包含顶点位置和法线。

            struct v2f
            {
                float4 pos : SV_POSITION;
            };
            //v2f：输出结构体，包含裁剪空间位置。

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(float4(v.vertex.xyz + v.normal * _OutlineWidth * 0.1, 1));
                return o;
            }
            //顶点着色器：将物体的顶点沿着法线方向扩展，产生描边效果。_OutlineWidth 控制描边的宽度。

            float4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            //片段着色器：渲染描边颜色 _OutlineColor。
            ENDHLSL

        }


        pass
        {
			Tags{ "LightMode" = "ShadowCaster" }

            //阴影 Pass：这个 Pass 用于处理阴影。它使用了 ShadowCaster 标签，表示它是一个专门为阴影计算而设计的 Pass。

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
			struct appdata
			{
				float4 vertex : POSITION;
			};
 
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
 
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				return o;
			}
            //顶点着色器：将顶点位置变换为裁剪空间（UNITY_MATRIX_MVP）以进行阴影计算。

			float4 frag(v2f i) : SV_Target
			{
				float4 color;
				color.xyz = float3(0.0, 0.0, 0.0);
				return color;
			}
            //片段着色器：输出黑色，表示阴影区域。

            //Main Pass：渲染物体的主纹理，使用 _BaseColor 混合。
            // Outline Pass：通过扩展顶点并绘制黑色轮廓，产生描边效果。
            //Shadow Pass：计算物体的阴影。
            //这个 Shader 是一个简单的 Toon 渲染 Shader，结合了基本的描边效果和阴影计算，可以应用于需要描边轮廓的 3D 模型。
			ENDHLSL
		}

    }
}

// Shader "Toon/BasicOutline" 
// {
//     Properties
//     {
//         _MainTex("main tex",2D) = ""{}
//         _Factor("factor",Range(0,0.1)) = 0.01//描边粗细因子
//         _OutLineColor("outline color",Color) = (0,0,0,1)//描边颜色
// 		_Color ("Main Tint", Color) = (1,1,1,0.1)
//     }
 
//     SubShader 
//     {
//         Pass
//         {
//             Cull Front //剔除前面
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #include "UnityCG.cginc"
 
//             struct v2f
//             {
//                 float4 vertex :POSITION;
//             };
 
//             float _Factor;
//             half4 _OutLineColor;
// 			fixed4 _Color;
 
//             v2f vert(appdata_full v)
//             {
//                 v2f o;
//                 //v.vertex.xyz += v.normal * _Factor;
//                 //o.vertex = mul(UNITY_MATRIX_MVP,v.vertex);
 
//                 //变换到视坐标空间下，再对顶点沿法线方向进行扩展
//                 float4 view_vertex = mul(UNITY_MATRIX_MV,v.vertex);
//                 float3 view_normal = mul((float3x3)UNITY_MATRIX_IT_MV,v.normal);
//                 view_vertex.xyz += normalize(view_normal) * _Factor; //记得normalize
//                 o.vertex = mul(UNITY_MATRIX_P,view_vertex);
// 				//fixed4 texColor = tex2D(_MainTex, i.uv);
//                 return o;
//             }
 
//             half4 frag(v2f IN):COLOR
//             {
//                 //return half4(0,0,0,1);
//                 return _OutLineColor;
//             }


//             ENDCG
//         }
 
//         Pass
//         {
//             Cull Back //剔除后面
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #include "UnityCG.cginc"
 
//             struct v2f
//             {
//                 float4 vertex :POSITION;
//                 float4 uv:TEXCOORD0;
//             };
 
//             sampler2D _MainTex;
 
//             v2f vert(appdata_full v)
//             {
//                 v2f o;
//                 o.vertex = UnityObjectToClipPos(v.vertex);
//                 o.uv = v.texcoord;
//                 return o;
//             }
 
//             half4 frag(v2f IN) :COLOR
//             {
//                 //return half4(1,1,1,1);
//                 half4 c = tex2D(_MainTex,IN.uv);
//                 return c;
//             }
//             ENDCG
//         }
//     } 
//     FallBack "Diffuse"
// }
