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
                #pragma shader_feature _MASKENABLE_ON
                #pragma shader_feature TEST_1 TEST_2
                //要生成未定义预处理器宏的着色器变体，请添加一个仅为下划线（__）的名称。这是避免使用两个宏的常用技术，因为对项目中可以使用的宏数量有限制，例如：
                //#pragma multi_compile __ _MASKENABLE_ON
                //#pragma multi_compile TEST_1 TEST_2

                #pragma multi_compile AAA BBB
                #pragma multi_compile CCC DDD EEE

                #pragma multi_compile_fwdbase
                
                //#pragma multi_compile_fog
                //假如将上述multi_compile替换为shader_feature：  我打包只打一个材质，这个材质用到了变体组合AC，那么打包时只会将AC打出来。
                //如果我的材质引用的是AE，那么会打出AC和AE，因为C是第二个Keyword声明组的默认Keyword，当你的材质用了这个Shader，
                //却没有发现没有引用这一声明组的任何一个Keyword（比如上面CDE都没引用），就会退化成第一个默认Keyword（上面的例子是C）。
                //所以一般声明Keyword组如果包含默认Keyword、关闭Keyword不会声明XXX_OFF，而是声明成 #pragma multi_compile _ C D，
                //这样如果材质引用AD，则会打出A和AD，不会减少变体数量，但可以减少Global Keyword的数量
                //multi_compile建议用于声明可能实时切换的全局Keyword声明组

                //一般情况下，项目会编写一个配置文件，里面记录各种需要剔除的变体条件，比如URP项目不需要BuildIn下的ForwardBasePass、DeferredPass，可以直接将这些Pass剔除掉，防止项目中有BuildIn下残留的变体。

                //NewShaderVariantsCollection  文件的作用有两个，其一是在打包时，对变体引用；其二是运行时，利用文件预热变体。
                
                //#pragma shader_feature AAA BBB
                //#pragma shader_feature CCC DDD EEE

                // Pass
                // {
                //     Tags{"LightMode" = "ShadowCaster"}
                //     #pragma shader_feature SHADOW_BIAS_ON
                //     #pragma shader_feature _ALPHATEST_ON
                // }
                //  
                // Pass
                // {
                //     Tags{"LightMode" = "UniversalForward"}
                //     #pragma shader_feature _ALPHATEST_ON
                //     #pragma shader_feature _NORMALMAP
                //     //....
                // }
                //此时，一个材质的ShaderKeywords中记录了SHADOW_BIAS_ON、_ALPHATEST_ON两个Keyword，那么材质就引用了SHADOW_BIAS_ON _ALPHATEST_ON和_ALPHATEST_ON这两个变体。

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

                    #if TEST_1
                        diff = 2;
                    #endif

                    #if _TEST_1
                        diff = 2;
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
