using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject targetObject; // 移動させる対象の物体
    public GameObject destinationObject; // 移動先のターゲット物体
    public float moveSpeed = 1.0f;  // 移動速度

    private bool shouldMove = false; // 移動を開始するかどうか

    void OnTriggerEnter(Collider other)
    {
        // トリガーがプレイヤーや他の特定の物体によって発動されたかを確認
        if (other.CompareTag("Player"))
        {
            // トリガーが発動されたとき、対象の物体を移動開始
            shouldMove = true;
        }
    }

    void Update()
    {
        // 移動フラグが立ったら、物体を平滑に移動
        if (shouldMove)
        {
            // 移動先のターゲット物体の位置を取得
            Vector3 targetPosition = destinationObject.transform.position;

            // MoveTowardsを使用して、対象の物体を目的の位置まで平滑に移動
            targetObject.transform.position = Vector3.MoveTowards(
                targetObject.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            // 目的の位置に到達したかを確認
            if (Vector3.Distance(targetObject.transform.position, targetPosition) < 0.01f)
            {
                shouldMove = false; // 移動を停止
            }
        }
    }
}
