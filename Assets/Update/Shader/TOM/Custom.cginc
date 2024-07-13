float inverseLerp(float a, float b, float value) {
    return saturate((value-a)/(b-a));
}

// HalfLambert拡散反射光を計算する
float3 CalcHalfLambertDiffuse(float3 lightDirection, float3 lightColor, float3 normal)
{
    // ピクセルの法線とライトの方向の内積を計算する
    float t = dot(normal, lightDirection);

    // 内積の値を0以上の値にする
    t = max(0.0f, t);

    t = pow(t * 0.5 + 0.5, 2);

    // 拡散反射光を計算する
    return lightColor * t;
}

// Phong鏡面反射光を計算する
float3 CalcPhongSpecular(float3 lightDirection, float3 lightColor, float3 toEye, float3 normal, float shinePower)
{
    // 反射ベクトルを求める
    float3 refVec = reflect(lightDirection, normal);

    // 光が当たったサーフェイスから視点に伸びるベクトルを求める
    toEye = normalize(toEye);

    // 鏡面反射の強さを求める
    float t = dot(refVec, toEye);

    // 鏡面反射の強さを0以上の数値にする
    t = max(0.0f, t);

    // 鏡面反射の強さを絞る
    t = pow(t, shinePower);

    // 鏡面反射光を求める
    return lightColor * t;
}
