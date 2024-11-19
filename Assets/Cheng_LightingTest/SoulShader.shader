Shader "Custom/SoulShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Color("本体の色", Color) = (1, 1, 1, 1) 

        [Header(Fresnel)]
        _FresnelColor ("フレネルの色", Color) = (0.5, 0.8, 1,1)
        _FresnelPower ("フレネルの強度", Range(0, 1)) = 0.5

        [Header(Transparency)]
        _Transparency ("透明度", Range(0,1)) = 0.5

        [Header(Emission)]
        [HideInInspector][HDR]_EmissionalColor ("エミションカラー", Color) = (0, 0, 0, 0)
        _EmissionalStrength ("エミションの強度", Range(0, 1 )) = 1.0


    }

    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);


            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;

            float4 _FresnelColor;

            float4 _Color;

            float _FresnelPower;

            float _Transparency;

            float4 _EmissionalColor;
            float _EmissionalStrength;

            CBUFFER_END



            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.normal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            float noise(float2 uv)
            {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float4 frag(v2f i) : SV_Target
            {
                //テクスチャカラー
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                //フレネル効果
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float fresnel = pow(1.0 - max(0.0, dot(viewDir, i.normal)), _FresnelPower);

                //合成
                float alpha = saturate(fresnel) * _Transparency;
                float3 finalColor = lerp(_Color.rgb, _FresnelColor.rgb, fresnel);
                col = float4(finalColor, alpha);

                _EmissionalColor = col;
                col += _EmissionalColor * _EmissionalStrength;

                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Transparent/Diffuse"
}
