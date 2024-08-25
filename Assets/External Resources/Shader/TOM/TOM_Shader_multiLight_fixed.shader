Shader "Custom/TOM_multiLight_fixed"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        //�m�[�}���}�b�v�Ή��p
        _BumpMap ("Bump Map", 2D) = "bump" {}
        _BumpScale("Normal scale", Range(0,2)) = 1

        //�H���P�A1�Ή��p
        _LimShadeColor1 ("�����A�̐F �x�[�X", Color) = (0,0,0,1)
        _LimShadeColorWeight1 ("�����A�F�̉e���x �x�[�X", Range(0, 1)) = 0.5
        _LimShadeMinPower1 ("�����A�̃O���f�͈� �x�[�X", Range(0, 1)) = 0.3
        _LimShadePowerWeight1 ("�ŔZ�����A�̑��� �x�[�X", Range(1, 10)) = 10

        //�H��2�A2�Ή��p
        _LimShadeColor2 ("�����A�̐F �O��", Color) = (0,0,0,1)
        _LimShadeColorWeight2 ("�����A�F�̉e���x �O��", Range(0, 1)) = 0.5
        _LimShadeMinPower2 ("�����A�̃O���f�͈� �O��", Range(0, 1)) = 0.3
        _LimShadePowerWeight2 ("�ŔZ�����A�̑��� �O��", Range(1, 10)) = 2

        //�H���R�A�̃}�X�N(�Ő�)�Ή��p
        _LimShadeMaskMinPower ("�����A�}�X�N�̃O���f�͈�", Range(0,1)) = 0.3
        _LimShadeMaskPowerWeight ("�ŔZ�����A�}�X�N�̑���", Range(1, 10)) = 2

        //�H���S�������C�g�Ή��p
        _LimLightWeight ("�������C�g�̉e���x", Range(0, 1)) = 0.5
        _LimLightPower ("�������C�g�̃O���f�͈�", Range(1, 5)) = 3

        //�H���T�V���h�E(�n�[�t�����o�[�g)�Ή��p
        _AmbientColor ("�����̐F", Color) = (0.5, 0.5, 0.5, 1)

        //�H���U�X�y�L����(����)�Ή��p
        _Smoothness ("�X���[�Y�l�X", Range(0, 1)) = 0.5
        _SpecularRate ("�X�y�L�����̉e���x", Range(0, 1)) = 0.3

        //�H���V�A�E�g���C���Ή��p
        _OutlineWidth ("�A�E�g���C���̑���", Range(0, 1)) = 0.1
        _OutlineColor ("�A�E�g���C���J���[", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipline"
            }
        LOD 300
        
        //�A�E�g���C���pPass

        Pass
        {
            // �O�ʂ��J�����O
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                half4 vertex : POSITION;
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

                //�A�E�g���C���̕������@�������Ɋg�傷��
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


        //�H���P�`�U�܂Ƃ�Pass
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
            //HLSL�̕W���֐��ɗp�ӂ���ĂȂ����̂����O�������Ă���
            #include "Custom.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                //�m�[�}���}�b�v�Ή��p�ǉ������o�[
                float3 normal : NORMAL;
                float4 tangent : TANGENT;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor: TEXCOORD1;
                float4 vertex : SV_POSITION;

                //�m�[�}���}�b�v�Ή��p�ǉ������o�[
                float3 normal : NORMAL;
                float2 uvNormal : TEXCOORD2;
                float3 tangent : TANGENT;
                float3 binormal : TEXCOORD3;

                //�H��1~3�Ή� - �A
                float3 viewDir : TEXCOORD4;

                //�H���U - �X�y�L����
                float3 toEye : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;

            //�m�[�}���}�b�v�Ή��p
            float4 _BumpMap_ST;
            float _BumpScale;

            //�H���P�A1�Ή��p
            float3 _LimShadeColor1;
            float _LimShadeColorWeight1;
            float _LimShadeMinPower1;
            float _LimShadePowerWeight1;

            //�H���Q�A2�Ή��p
            float3 _LimShadeColor2;
            float _LimShadeColorWeight2;
            float _LimShadeMinPower2;
            float _LimShadePowerWeight2;

            //�H���R�A�̃}�X�N(�Ő�)�Ή��p
            float _LimShadeMaskMinPower;
            float _LimShadeMaskPowerWeight;

            //�H���S�������C�g�Ή��p
            float _LimLightWeight;
            float _LimLightPower;

            //�H���T�V���h�E(�n�[�t�����o�[�g)�Ή��p
            float3 _AmbientColor;

            //�H���U�X�y�L����(����)
            float _Smoothness;
            float _SpecularRate;

            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);

                //�m�[�}���}�b�v�Ή�
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.uvNormal = TRANSFORM_TEX(v.uv, _BumpMap);
                o.tangent = v.tangent;
                o.tangent.xyz = TransformObjectToWorldDir(v.tangent.xyz);
                o.binormal = normalize(cross(v.normal, v.tangent.xyz) * v.tangent.w * unity_WorldTransformParams.w);

                //�H���P�`�R - �A
                o.viewDir = normalize(-GetViewForwardDir());

                //�H���U - �X�y�L����(����)
                o.toEye = normalize(GetWorldSpaceViewDir(TransformObjectToWorld(v.vertex.xyz)));

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                //apply fog
                col.rgb = MixFog(col.rgb, i.fogFactor);

                //�e�N�X�`������擾�����I���W�i���̐F��ێ�
                float4 albedo = col;

                //�m�[�}���}�b�v�Ή�
                //�m�[�}���}�b�v����@�������擾����
                float3 localNormal = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.uvNormal), _BumpScale);
                //�_���W�F���g�X�y�[�X�̖@�������[���h�X�y�[�X�ɕϊ�����
                i.normal = i.tangent * localNormal.x + i.binormal * localNormal.y + i.normal * localNormal.z;
                
                //���C�e�B���O�����Q�b�g����
                Light light = GetMainLight();

                //�H���P�A1�̌v�Z������
                float limPower = 1 - max(0, dot(i.normal, i.viewDir));
                float limShadePower = inverseLerp(_LimShadeMinPower1, 1, limPower);
                limShadePower = min(limShadePower * _LimShadePowerWeight1, 1);
                col.rgb = lerp(col.rgb, col.rgb * _LimShadeColor1, limShadePower * _LimShadeColorWeight1);

                //�H���Q�A2�̌v�Z������
                limShadePower = inverseLerp(_LimShadeMinPower2, 1, limPower);
                limShadePower = min(limShadePower * _LimShadePowerWeight2, 1);
                col.rgb = lerp(col.rgb, albedo.rgb * _LimShadeColor2, limShadePower * _LimShadeColorWeight2);

                //�H���R�A�̃}�X�N(�Ő�)
                float limShadeMaskPower = inverseLerp(_LimShadeMaskMinPower, 1, limPower);
                limShadeMaskPower = min(limShadeMaskPower * _LimShadeMaskPowerWeight, 1);
                col.rgb = lerp(col.rgb, albedo.rgb, limShadeMaskPower);

                //�H���S�������C�g
                
                float limLightPower = 1 - max(0, dot(i.normal, -light.direction));
                float3 limLight = pow(saturate(limPower * limLightPower), _LimLightPower) * light.color;
                col.rgb += limLight * _LimLightWeight;

                //�H���T�V���h�E(�n�[�t�����o�[�g)
                float3 diffuseLight = CalcHalfLambertDiffuse(light.direction, light.color, i.normal);

                //�H���U�X�y�L����(����)
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
