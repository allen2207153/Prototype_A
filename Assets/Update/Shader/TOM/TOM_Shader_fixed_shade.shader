Shader "Custom/TOM_fixed_shade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        //ノーマルマップ対応用
        _BumpMap ("Bump Map", 2D) = "bump" {}
        _BumpScale("Normal scale", Range(0,2)) = 1

        //工程１陰1対応用
        _LimShadeColor1 ("リム陰の色 ベース", Color) = (0,0,0,1)
        _LimShadeColorWeight1 ("リム陰色の影響度 ベース", Range(0, 1)) = 0.5
        _LimShadeMinPower1 ("リム陰のグラデ範囲 ベース", Range(0, 1)) = 0.3
        _LimShadePowerWeight1 ("最濃リム陰の太さ ベース", Range(1, 10)) = 10

        //工程2陰2対応用
        _LimShadeColor2 ("リム陰の色 外側", Color) = (0,0,0,1)
        _LimShadeColorWeight2 ("リム陰色の影響度 外側", Range(0, 1)) = 0.5
        _LimShadeMinPower2 ("リム陰のグラデ範囲 外側", Range(0, 1)) = 0.3
        _LimShadePowerWeight2 ("最濃リム陰の太さ 外側", Range(1, 10)) = 2

        //工程３陰のマスク(稜線)対応用
        _LimShadeMaskMinPower ("リム陰マスクのグラデ範囲", Range(0,1)) = 0.3
        _LimShadeMaskPowerWeight ("最濃リム陰マスクの太さ", Range(1, 10)) = 2

        //工程４リムライト対応用
        _LimLightWeight ("リムライトの影響度", Range(0, 1)) = 0.5
        _LimLightPower ("リムライトのグラデ範囲", Range(1, 5)) = 3

        //工程５シャドウ(ハーフランバート)対応用
        _AmbientColor ("環境光の色", Color) = (0.5, 0.5, 0.5, 1)

        //工程６スペキュラ(光沢)対応用
        _Smoothness ("スムーズネス", Range(0, 1)) = 0.5
        _SpecularRate ("スペキュラの影響度", Range(0, 1)) = 0.3

        //工程７アウトライン対応用
        _OutlineWidth ("アウトラインの太さ", Range(0, 1)) = 0.1
        _OutlineColor ("アウトラインカラー", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipline"
            }
        LOD 100
        
        //アウトライン用Pass

        Pass
        {
            // 前面をカリング
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                half4 vertex : SV_POSITION;
                half3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;

            half _OutlineWidth;
            half4 _OutlineColor;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;

                //アウトラインの分だけ法線方向に拡大する
                o.vertex = TransformObjectToHClip(v.vertex + v.normal * (_OutlineWidth / 100));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                return col * _OutlineColor;
            }
            ENDHLSL
        }


        //工程１～６まとめPass
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //make fog work
            #pragma multi_compile_fog 

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            //HLSLの標準関数に用意されてないものを自前実装している
            #include "Custom.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                //ノーマルマップ対応用追加メンバー
                float3 normal : NORMAL;
                float4 tangent : TANGENT;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor: TEXCOORD1;
                float4 vertex : SV_POSITION;

                //ノーマルマップ対応用追加メンバー
                float3 normal : NORMAL;
                float2 uvNormal : TEXCOORD2;
                float3 tangent : TANGENT;
                float3 binormal : TEXCOORD3;

                //工程1~3対応 - 陰
                float3 viewDir : TEXCOORD4;

                //工程６ - スペキュラ
                float3 toEye : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;

            //ノーマルマップ対応用
            float4 _BumpMap_ST;
            float _BumpScale;

            //工程１陰1対応用
            float3 _LimShadeColor1;
            float _LimShadeColorWeight1;
            float _LimShadeMinPower1;
            float _LimShadePowerWeight1;

            //工程２陰2対応用
            float3 _LimShadeColor2;
            float _LimShadeColorWeight2;
            float _LimShadeMinPower2;
            float _LimShadePowerWeight2;

            //工程３陰のマスク(稜線)対応用
            float _LimShadeMaskMinPower;
            float _LimShadeMaskPowerWeight;

            //工程４リムライト対応用
            float _LimLightWeight;
            float _LimLightPower;

            //工程５シャドウ(ハーフランバート)対応用
            float3 _AmbientColor;

            //工程６スペキュラ(光沢)
            float _Smoothness;
            float _SpecularRate;

            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);

                //ノーマルマップ対応
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.uvNormal = TRANSFORM_TEX(v.uv, _BumpMap);
                o.tangent = v.tangent;
                o.tangent.xyz = TransformObjectToWorldDir(v.tangent.xyz);
                o.binormal = normalize(cross(v.normal, v.tangent.xyz) * v.tangent.w * unity_WorldTransformParams.w);

                //工程１～３ - 陰
                o.viewDir = normalize(-GetViewForwardDir());

                //工程６ - スペキュラ(光沢)
                o.toEye = normalize(GetWorldSpaceViewDir(TransformObjectToWorld(v.vertex.xyz)));

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                //apply fog
                col.rgb = MixFog(col.rgb, i.fogFactor);

                //テクスチャから取得したオリジナルの色を保持
                float4 albedo = col;

                //ノーマルマップ対応
                //ノーマルマップから法線情報を取得する
                float3 localNormal = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.uvNormal), _BumpScale);
                //ダンジェントスペースの法線をワールドスペースに変換する
                i.normal = i.tangent * localNormal.x + i.binormal * localNormal.y + i.normal * localNormal.z;
                
                //ライティング情報をゲットする
                Light light = GetMainLight();

                //工程１陰1の計算をする
                float limPower = 1 - max(0, dot(i.normal, i.viewDir));
                float limShadePower = inverseLerp(_LimShadeMinPower1, 1, limPower);
                limShadePower = min(limShadePower * _LimShadePowerWeight1, 1);
                col.rgb = lerp(col.rgb, col.rgb * _LimShadeColor1, limShadePower * _LimShadeColorWeight1);

                //工程２陰2の計算をする
                limShadePower = inverseLerp(_LimShadeMinPower2, 1, limPower);
                limShadePower = min(limShadePower * _LimShadePowerWeight2, 1);
                col.rgb = lerp(col.rgb, albedo.rgb * _LimShadeColor2, limShadePower * _LimShadeColorWeight2);

                //工程３陰のマスク(稜線)
                float limShadeMaskPower = inverseLerp(_LimShadeMaskMinPower, 1, limPower);
                limShadeMaskPower = min(limShadeMaskPower * _LimShadeMaskPowerWeight, 1);
                col.rgb = lerp(col.rgb, albedo.rgb, limShadeMaskPower);

                //工程４リムライト
                
                float limLightPower = 1 - max(0, dot(i.normal, -light.direction));
                float3 limLight = pow(saturate(limPower * limLightPower), _LimLightPower) * light.color;
                col.rgb += limLight * _LimLightWeight;

                //工程５シャドウ(ハーフランバート)
                float3 diffuseLight = CalcHalfLambertDiffuse(light.direction, light.color, i.normal);

                //工程６スペキュラ(光沢)
                float shinePower = lerp(0.5, 10, _Smoothness);
                float3 specularLight = CalcPhongSpecular(-light.direction, light.color, i.toEye, i.normal, shinePower);
                specularLight = lerp(0, specularLight, _SpecularRate);
                                
                col.rgb *= diffuseLight + specularLight + _AmbientColor ;
                

                return col;
            }
            ENDHLSL
        }
    }
}
