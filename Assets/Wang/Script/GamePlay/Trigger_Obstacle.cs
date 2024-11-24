using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 障害物のトリガーアクションを管理するクラス
public class Trigger_Obstacle : MonoBehaviour, IPlayerTriggerAction
{
    public ObjectFallController objectFallController; // オブジェクト落下のコントローラー
    private bool playerInRange = false;               // プレイヤーが範囲内にいるかどうかを判定するフラグ
    private bool activeEvent;


    void Start()
    {
        activeEvent = GameObject.Find("Oniisan").GetComponent<PlayerMovement>().interactionActive;
    }
    // プレイヤーのトリガーアクションを開始するメソッド
    public void TriggerAction(PlayerMovement playerMovement)
    {
        playerMovement.SetInteractionTrigger(true); // プレイヤーのインタラクショントリガーを設定
    }

    // プレイヤーのトリガーアクションを終了するメソッド
    public void EndAction(PlayerMovement playerMovement)
    {
        playerMovement.SetInteractionTrigger(false); // インタラクショントリガーを解除
    }

    // トリガーにプレイヤーが入ったときの処理
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WAAA");
            playerInRange = true; // プレイヤーが範囲内にいることをフラグで管理
            objectFallController.TriggerFall();
            
        }
    }

    // トリガーからプレイヤーが出たときの処理
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // プレイヤーが範囲外に出たことをフラグで管理
            // 自分自身を無効化
            DisableTrigger();
        }
    }

    private void DisableTrigger()
    { 
        gameObject.SetActive(false);
    }

    // ギズモを描画してトリガー範囲を可視化するメソッド
    private void OnDrawGizmos()
    {
        var boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size); // トリガー範囲を黄色い枠で表示
        }
    }
}