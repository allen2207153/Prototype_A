using UnityEngine;

public class PillarShake : MonoBehaviour
{
    // 揺れの振幅と周波数
    public float shakeAmplitude = 0.5f; // 揺れの振幅
    public float shakeFrequency = 2.0f; // 揺れの周波数

    // 元の位置
    private Vector3 originalPosition;

    void Start()
    {
        // 柱の元の位置を記録
        originalPosition = transform.position;
    }

    void Update()
    {
        // 揺れの効果を計算（正弦波を使用して滑らかな揺れを生成）
        float shakeOffset = Mathf.Sin(Time.time * shakeFrequency) * shakeAmplitude;

        // 柱のY軸位置を更新（必要に応じてX軸やZ軸に変更可能）
        transform.position = new Vector3(originalPosition.x, originalPosition.y + shakeOffset, originalPosition.z);
    }
}
