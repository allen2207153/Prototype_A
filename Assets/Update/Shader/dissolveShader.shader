Shader "Unlit/dissolveShader"
{
    Properties
    {//追加パラメーター
     //インスペクターに表示し、外部、コードからアクセス可能にする

        //_baseColor("Color", Color) = (1,1,1)
        //「Color」変数の「_baseColor」外部では「Color」デフォルトの値は「(1,1,1,1)」(白)
        _dissolveColor("DamageColor", Color) = (0, 0, 0)
        //「Color」変数の「_dissolveColor」外部では「DamageColor」デフォルトの値は「(0,0,0,1)」(黒)
        //Dissolveで変化する色
        _mainTex("Texture", 2D) = "white" {}
        //「2D」変数の「_mainTex」外部では「Texture」デフォルトの値は「white」
        _dissolveTex("Dissolve Texture", 2D) = "white" {}
        //「2D」変数の「_dissolveTex」外部では「Dissolve Texture」デフォルトの値は「white」
        //Dissoveに使うノイズの画像、画像が設定されていない場合白い画像が使われる
        _alphaClipThreshold("DissolveRate", Range(0,1)) = 0.5
        //「Range」変数の「_alphaClipThreshold」外部では「DissolveRate」デフォルトの値は「0.5」
    }
        SubShader
    {//シェーダーの中身の記述
        Tags { "RenderType" = "Opaque" }
        //シェーダーの設定
        //Opqaueは不透明。半透明なども指定が出来る
        LOD 100
        //Level of Detail 
        //カメラとの距離で処理分岐をさせたい場合の閾値、使うかわからない
        Pass
        {
            CGPROGRAM
            //ここからシェーダーの中身だという宣言
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            //Unity側で予め用意されている関数などを使用可能にする

            struct appdata
            {//シェーダーの設定しているモデルから受け取る数値とその定義

                float4 vertex : POSITION;
                //モデルのPOSITION(頂点の位置)を受け取り利用する
                float2 uv : TEXCOORD0;
                //モデルのTEXCOORD0を受け取り利用する
            };

            struct v2f
            {//vertex to fragment
             //頂点シェーダーからフラグメントシェーダーへの値の引き渡し

                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _dissolveColor;
            //ベースの色
            half _alphaClipThreshold;
            //Dissolveで変化する色
            sampler2D _mainTex;
            //ベースのテクスチャ画像
            float4 _mainTex_ST;

            sampler2D _dissolveTex;
            //Dissoveに使うノイズのテクスチャ画像
            float4 _dissolveTex_ST;

            v2f vert(appdata v)
            {//vertex（頂点）シェーダー時点での処理

                v2f o;
                //頂点情報の戻り値
                o.vertex = UnityObjectToClipPos(v.vertex);
                // UnityCG.cgincで定義されてる関数
                //3D空間においてのスクリーン上のでの座標変換を行う関数
                o.uv = TRANSFORM_TEX(v.uv, _mainTex);
                //UnityCG.cgincで定義されてる関数
                //テクスチャとUV座標を関連付ける関数
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
           {//fragment(フラグメント)シェーダー時点の処理
            //v2fから受け取った値を基に処理する

            fixed4 dissolveCol = fixed4(1, 1, 1, 1);
            //初期のdissolveColを白にする

            fixed4 dissolve = tex2D(_dissolveTex, i.uv);
            //tex2D関数はUV座標からテクスチャの色を取得する
            //ノイズのテクスチャから色の情報を取得
            float alpha = dissolve.r * 0.2 + dissolve.g * 0.7 + dissolve.b * 0.1;
            // noise textureからalpha値を取得


            if (alpha < _alphaClipThreshold)
            {
                dissolveCol = _dissolveColor;
            }
            //ノイズのテクスチャから取得したalpha値より小さい画素を黒くする
            fixed4 col = tex2D(_mainTex, i.uv) * dissolveCol;
            return col;
          }
        ENDCG
          }
        //シェーダーの終わりの宣言
    }
}
