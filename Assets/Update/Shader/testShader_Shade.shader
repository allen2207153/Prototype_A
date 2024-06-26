Shader "Custom/testShader_Shade"
{
    // プロパティセクション
    //   マテリアルの外部から調整可能なパラメータを定義します。
    Properties
    {
        //カスタムプロパティ "_Color"の宣言
        _Color("Color", Color) = (1, 1, 1, 1)

        //カスタムプロパティ "_Strength"の宣言
        _Strength("Strength", Range(0, 1)) = 0.4
    }

    // サブシェーダーセクション
    //   ここにシェーダーの実際の描画ロジックを記述します。
    SubShader
    {
        //タグ設定：レンダリングキューを透明に設定
        //タグ設定：URP対応
        Tags {
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
             }

        //ブレンドモードの設定：アルファブレンド
        Blend SrcAlpha OneMinusSrcAlpha
       

        //シェーダーのマスを定義
        Pass
        {
            //パスの名前
            Name "COLOR_SHADE"

            //プログラマブルなシェーダーの開始宣言
            HLSLPROGRAM

            //頂点シェーダーの設定
            #pragma vertex vert

            //フラグメントシェーダーの指定
            #pragma fragment frag

            //Universal Pipeline shadow keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            //UnityCG.cgincライブラリをインクルード
            //#include "UnityCG.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
            //プロパティで定義されたシェーダーの強度を定義する変数
            float _Strength;

            //プロパティで定義されたカラー情報を格納する変数
            float4 _Color;
            CBUFFER_END

            //頂点情報を格納する構造体の宣言
            struct appdata
            {
                //頂点の座標情報
                float4 vertex : POSITION;
                //頂点の法線情報
                float3 normal : NORMAL;
            };

            //頂点シェーダーからフラグメントシェーダーにデータを渡す
            struct v2f
            {
                //頂点のスクレーン座標
                float4 pos : SV_POSITION;
                //ワールド空間の法線情報
                float3 worldNormal : TEXCOORD0;
            };

            //頂点シェーダー関数の宣言
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex); // 頂点位置をクリップ座標に変換
                o.worldNormal = TransformObjectToWorldNormal(v.normal); //法線情報をワールド空間に変換
                return o;
            };

            

            //フラグメントシェーダー関数の宣言
            //fixed4はHLSLに対応しないためhalf4に変換する
            half4 frag(v2f i) : SV_Target
            {
                //ライトの方向ベクトルを計算
                //
                Light l = GetMainLight(); 

                //法線ベクトルを計算
                float3 n = normalize(i.worldNormal); 

                //ライトと法線ベクトルの内積を取得し、補間値を計算
                float interpolation = step(dot(n, 1), 0); 

                //最終的なカラーを計算
                float4 final_color = lerp(_Color, (1 - _Strength) * _Color, interpolation); 

                //アルファ値をカスタマプロパティのアルファ値に設定
                final_color.a = _Color.a; 

                return final_color; //最終出力カラーを返す
            }

            //プログラマブルなシェーダーの終了宣言
            ENDHLSL
        }
    }

    // フォールバック
    //   このシェーダーが使用できない場合、
    //   デフォルトの "Standard" シェーダーを使用します。
Fallback"Standard"
}
