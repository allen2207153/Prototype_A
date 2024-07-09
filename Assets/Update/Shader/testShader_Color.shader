Shader "Custom/testShader_Color"
{
     // プロパティセクション
    Properties
    {
        // カスタムプロパティ "_Color" の宣言
        _Color("Color", Color) = (1, 1, 1, 1)
    }

    // サブシェーダーセクション
    SubShader
    {
        // タグの設定: レンダリングキューを透明に設定
        Tags { "Queue" = "Transparent" }
        
        // ブレンドモードの設定: アルファブレンド
        Blend SrcAlpha OneMinusSrcAlpha
        
        // シェーダーのパスを定義
        Pass
        {
            // パスの名前
            Name "COLOR"
            
            // プログラマブルなシェーダーの開始宣言
            CGPROGRAM
            
            // 頂点シェーダーの指定
            #pragma vertex vert
            
            // フラグメントシェーダーの指定
            #pragma fragment frag

            // 頂点情報を格納する構造体の宣言
            struct appdata
            {
                // 頂点の座標情報
                float4 vertex : POSITION;
            };

            // 頂点シェーダーからフラグメントシェーダーにデータを渡す構造体の宣言
            struct v2f
            {
                // 頂点のスクリーン座標
                float4 pos : SV_POSITION;
            };

            // 頂点シェーダー関数の宣言
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // 頂点位置をクリップ座標に変換
                return o;
            }

            // プロパティで定義されたカラー情報を格納する変数
            float4 _Color;

            // フラグメントシェーダー関数の宣言
            fixed4 frag(v2f i) : SV_Target
            {
                float4 final_color = _Color; // シェーダーの最終出力カラーを定義
                final_color.a = _Color.a; // アルファ値をカスタムプロパティのアルファ値に設定
                return final_color; // 最終出力カラーを返す
            }
            
            // プログラマブルなシェーダーの終了宣言
            ENDCG
        }
    }

    // フォールバック
    Fallback "Standard"
}
