using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public TriggerRotate targetRotatingObject; // 回転させたいオブジェクト

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーがボタンの範囲に入ったとき
        {
            targetRotatingObject.ActivateTrigger(); // 回転を開始
        }
    }
}
