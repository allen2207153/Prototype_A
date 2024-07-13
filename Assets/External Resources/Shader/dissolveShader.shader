Shader "Unlit/dissolveShader"
{
    Properties
    {//�ǉ��p�����[�^�[
     //�C���X�y�N�^�[�ɕ\�����A�O���A�R�[�h����A�N�Z�X�\�ɂ���

        //_baseColor("Color", Color) = (1,1,1)
        //�uColor�v�ϐ��́u_baseColor�v�O���ł́uColor�v�f�t�H���g�̒l�́u(1,1,1,1)�v(��)
        _dissolveColor("DamageColor", Color) = (0, 0, 0)
        //�uColor�v�ϐ��́u_dissolveColor�v�O���ł́uDamageColor�v�f�t�H���g�̒l�́u(0,0,0,1)�v(��)
        //Dissolve�ŕω�����F
        _mainTex("Texture", 2D) = "white" {}
        //�u2D�v�ϐ��́u_mainTex�v�O���ł́uTexture�v�f�t�H���g�̒l�́uwhite�v
        _dissolveTex("Dissolve Texture", 2D) = "white" {}
        //�u2D�v�ϐ��́u_dissolveTex�v�O���ł́uDissolve Texture�v�f�t�H���g�̒l�́uwhite�v
        //Dissove�Ɏg���m�C�Y�̉摜�A�摜���ݒ肳��Ă��Ȃ��ꍇ�����摜���g����
        _alphaClipThreshold("DissolveRate", Range(0,1)) = 0.5
        //�uRange�v�ϐ��́u_alphaClipThreshold�v�O���ł́uDissolveRate�v�f�t�H���g�̒l�́u0.5�v
    }
        SubShader
    {//�V�F�[�_�[�̒��g�̋L�q
        Tags { "RenderType" = "Opaque" }
        //�V�F�[�_�[�̐ݒ�
        //Opqaue�͕s�����B�������Ȃǂ��w�肪�o����
        LOD 100
        //Level of Detail 
        //�J�����Ƃ̋����ŏ�����������������ꍇ��臒l�A�g�����킩��Ȃ�
        Pass
        {
            CGPROGRAM
            //��������V�F�[�_�[�̒��g���Ƃ����錾
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            //Unity���ŗ\�ߗp�ӂ���Ă���֐��Ȃǂ��g�p�\�ɂ���

            struct appdata
            {//�V�F�[�_�[�̐ݒ肵�Ă��郂�f������󂯎�鐔�l�Ƃ��̒�`

                float4 vertex : POSITION;
                //���f����POSITION(���_�̈ʒu)���󂯎�藘�p����
                float2 uv : TEXCOORD0;
                //���f����TEXCOORD0���󂯎�藘�p����
            };

            struct v2f
            {//vertex to fragment
             //���_�V�F�[�_�[����t���O�����g�V�F�[�_�[�ւ̒l�̈����n��

                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _dissolveColor;
            //�x�[�X�̐F
            half _alphaClipThreshold;
            //Dissolve�ŕω�����F
            sampler2D _mainTex;
            //�x�[�X�̃e�N�X�`���摜
            float4 _mainTex_ST;

            sampler2D _dissolveTex;
            //Dissove�Ɏg���m�C�Y�̃e�N�X�`���摜
            float4 _dissolveTex_ST;

            v2f vert(appdata v)
            {//vertex�i���_�j�V�F�[�_�[���_�ł̏���

                v2f o;
                //���_���̖߂�l
                o.vertex = UnityObjectToClipPos(v.vertex);
                // UnityCG.cginc�Œ�`����Ă�֐�
                //3D��Ԃɂ����ẴX�N���[����̂ł̍��W�ϊ����s���֐�
                o.uv = TRANSFORM_TEX(v.uv, _mainTex);
                //UnityCG.cginc�Œ�`����Ă�֐�
                //�e�N�X�`����UV���W���֘A�t����֐�
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
           {//fragment(�t���O�����g)�V�F�[�_�[���_�̏���
            //v2f����󂯎�����l����ɏ�������

            fixed4 dissolveCol = fixed4(1, 1, 1, 1);
            //������dissolveCol�𔒂ɂ���

            fixed4 dissolve = tex2D(_dissolveTex, i.uv);
            //tex2D�֐���UV���W����e�N�X�`���̐F���擾����
            //�m�C�Y�̃e�N�X�`������F�̏����擾
            float alpha = dissolve.r * 0.2 + dissolve.g * 0.7 + dissolve.b * 0.1;
            // noise texture����alpha�l���擾


            if (alpha < _alphaClipThreshold)
            {
                dissolveCol = _dissolveColor;
            }
            //�m�C�Y�̃e�N�X�`������擾����alpha�l��菬������f����������
            fixed4 col = tex2D(_mainTex, i.uv) * dissolveCol;
            return col;
          }
        ENDCG
          }
        //�V�F�[�_�[�̏I���̐錾
    }
}
