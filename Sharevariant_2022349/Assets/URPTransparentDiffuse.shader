Shader "Custom/URPTransparentDiffuse"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        
        [Header(Mask)]
        //用一个开关来控制 shader 的变种，即效果就是控制 遮罩效果的是否生效
        [Toggle]_MaskEnable("Mask Enabled",int) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //根据对应的开关 来定义用于shader变种的预编译 条件（大写加_ON）
            //#pragma shader_feature _MASKENABLE_ON
            //要生成未定义预处理器宏的着色器变体，请添加一个仅为下划线（__）的名称。这是避免使用两个宏的常用技术，因为对项目中可以使用的宏数量有限制，例如：
            #pragma multi_compile __ _MASKENABLE_ON
            #pragma multi_compile TEST_1 TEST_2
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normal = normalize(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv);
                half3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                half3 worldNormal = normalize(i.normal);
                half3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

                // Lambertian lighting
                half diff = max(0, dot(worldNormal, worldLightDir));

                 #if _MASKENABLE_ON
                    diff = 1;
                #endif

                
                half4 finalColor = texColor * _Color;
                finalColor.rgb *= ambient + diff;

                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack "Legacy Shaders/Transparent/Diffuse"
}
