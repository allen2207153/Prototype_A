using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingObject : MonoBehaviour
{
    public float shakeIntensity = 0.1f;    // 振動の強さ
    public float shakeFrequency = 20.0f;   // 振動の頻度
    public float dropDelay = 2.0f;         // 振動後の落下までの遅延時間

    private Vector3 initialPosition;       // 初期位置を保存
    private Rigidbody rb;                  // Rigidbodyの参照

    void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;

        // Rigidbodyコンポーネントを取得し、重力を無効にする
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // 初期状態で重力を無効にする
        }

        // 振動と遅延後の落下を開始
        StartCoroutine(ShakeAndDrop());
    }

    void Update()
    {
        // Updateでは何も処理しない、コルーチンで制御
    }

    // 振動と自動落下を制御するコルーチン
    private IEnumerator ShakeAndDrop()
    {
        // 振動を開始するために現在の時刻を基に振動計算
        float startTime = Time.time;

        // dropDelay秒間振動
        while (Time.time < startTime + dropDelay)
        {
            Shake();
            yield return null; // 次のフレームまで待機
        }

        // 振動が終了したら重力を有効化して物体を落下させる
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }

    // 振動効果を実行するメソッド
    void Shake()
    {
        float x = Mathf.Sin(Time.time * shakeFrequency) * shakeIntensity;
        float y = Mathf.Sin(Time.time * shakeFrequency * 1.1f) * shakeIntensity; // 周波数を少しずらして自然に

        transform.position = initialPosition + new Vector3(x, y, 0);
    }
}
