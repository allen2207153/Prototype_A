using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrigger : MonoBehaviour
{
    public float slowSpeed = 1.5f;  // トリガーに触れたときの遅い速度
    public float normalSpeed = 3.5f; // 通常の速度
    private PlayerMovement playerMovement;  // プレイヤーの移動スクリプトへの参照

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに触れた場合
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetMaxSpeed(slowSpeed);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーがトリガーから離れた場合
        if (other.CompareTag("Player"))
        {
            if (playerMovement != null)
            {
                playerMovement.SetMaxSpeed(normalSpeed); // 元の速度に戻す
            }
        }
    }
}
