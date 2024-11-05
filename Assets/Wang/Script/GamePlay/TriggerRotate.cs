using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 30, 0); // 毎秒の回転速度 (XYZ)
    public bool isTriggered = false; // トリガーが発動したかどうか
    public float rotationDuration = 5f; // 回転が続く時間
    private float rotationTime = 0f; // 回転のタイマー

    void Update()
    {
        // トリガーが発動し、まだ時間内であれば回転する
        if (isTriggered && rotationTime < rotationDuration)
        {
            // オブジェクトを回転させる
            transform.Rotate(rotationSpeed * Time.deltaTime);
            rotationTime += Time.deltaTime; // タイマーを更新
        }
    }

    // トリガーが有効化されたときに呼ばれるメソッド（例: プレイヤーがボタンを押したとき）
    public void ActivateTrigger()
    {
        isTriggered = true; // 回転を開始
        rotationTime = 0f; // タイマーをリセット
    }
}
