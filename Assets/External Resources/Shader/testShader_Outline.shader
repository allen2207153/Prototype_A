Shader "Custom/testShader_Outline"
{
    // プロパティセクション
    //   マテリアルの外部から調整可能なパラメータを定義します。
    Properties
    {
        //カスタムプロパティ "_Color"の宣言
        _Color("Color", Color) = (1, 1, 1, 1)

        //カスタムプロパティ "_Strength"の宣言
        _Strength("Strength", Range(0, 1)) = 0.2

        //カスタムプロパティ "_OutlineWidth"の宣言
        _OutlineWidth("Outline width", Range(0.0001, 0.03)) = 0.0005

        //カスタムプロパティ "_OutlineColor"の宣言
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)

        //カスタムプロパティ "_UseVertexExpansion"の宣言
        [Toggle(USE_VERTEX_EXPANSION)] _UseVertexExpansion("Use Vertex for outline", int) = 0
    }

    // サブシェーダーセクション
    //   ここにシェーダーの実際の描画ロジックを記述します。
    SubShader
    {
        //"Custom/testShader_Shade/COLOR_SHADE" パスを使用
        UsePass "Custom/testShader_Shade/COLOR_SHADE"

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
            Tags{ "LightMode" = "UniversalForward" }

            //カリングをフロントに設定
            Cull Front

            //プログラマブルなシェーダーの開始宣言
            HLSLPROGRAM

            //頂点シェーダーの設定
            #pragma vertex vert

            //フラグメントシェーダーの指定
            #pragma fragment frag

            //"USE_VERTEXT_EXPANSION" シェーダーフィーチャーを有効か
            #pragma shader_feature USE_VERTEX_EXPANSION

            //UnityCG.cgincライブラリをインクルード
            //#include "UnityCG.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //プロパティで定義されたアウトラインの幅を定義する変数
            float _OutlineWidth;

            CBUFFER_START(UnityPerMaterial)
            //プロパティで定義されたアウトラインのカラーを定義する変数
            float4 _OutlineColor;

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

            //頂点シェーダーからフラグメントシェーダーにデータを渡す構造体の宣言
            struct v2f
            {
                //頂点のスクレーン座標
                float4 pos : SV_POSITION;
            };

            //頂点シェーダー関数の宣言
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex); // 頂点位置をクリップ座標に変換
                float3 n = 0; //法線ベクトルの初期化
                
                //USE_VERTEXT_EXPANSION フェイーチャーが有効な場合、頂点を拡張法線を使用して計算
                #ifdef USE_VERTEX_EXPANSION

                //頂点の位置から方向ベクトルを計算
                float3 dir = normalize(v.vertex.xyz);

                //拡張法線をワールド空間に変換
                n = normalize(mul((float3x3) UNITY_MATRIX_IT_MV, dir));

                //USE_VERTEXT_EXPANSION フィーチャーが無効な場合、通帳の法専を使用して計算
                #else

                //法線ベクトルをワールド空間に変換
                n = normalize(mul((float3x3) UNITY_MATRIX_IT_MV, v.normal));

                #endif

                //法線情報からビューからプロジェクション座標に変換
                //float2 offset = TransformViewToProjection(n.xy);
                float2 offset = mul((float2x2)UNITY_MATRIX_P, n.xy);

                //座標にアウトラインのオフセットを追加
                o.pos.xy += offset * _OutlineWidth;

                return o;
            };

            //フラグメントシェーダー関数の宣言
            //fixed4はHLSLに対応しないためhalf4に変換する
            half4 frag(v2f i) : SV_Target
            {
                //最終的なカラーをアウトラインカラーで初期化
                float4 final_color = _OutlineColor; 

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
