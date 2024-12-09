using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 障害物のトリガーアクションを管理するクラス
public class Trigger_Obstacle : MonoBehaviour
{
    public ObjectFallController objectFallController; // オブジェクト落下のコントローラー
    private bool _isPlayerInRange = false;               // プレイヤーが範囲内にいるかどうかを判定するフラグ
    //private bool activeEvent;

    private void OnEnable()
    {
        EventSystem.Instance.StartListening(GameEvents.PushTheBrige, PushTheBrige);
    }
    private void OnDisable()
    {
        EventSystem.Instance.StopListening(GameEvents.PushTheBrige, PushTheBrige);
    }
    //void Start()
    //{
    //    //activeEvent = GameObject.Find("Oniisan").GetComponent<PlayerMovement>().interactionActive;
    //}
    // プレイヤーのトリガーアクションを開始するメソッド
    //public void TriggerAction(PlayerMovement playerMovement)
    //{
    //    playerMovement.SetInteractionTrigger(true); // プレイヤーのインタラクショントリガーを設定
    //}

    //// プレイヤーのトリガーアクションを終了するメソッド
    //public void EndAction(PlayerMovement playerMovement)
    //{
    //    playerMovement.SetInteractionTrigger(false); // インタラクショントリガーを解除
    //}

    //トリガーにプレイヤーが入ったときの処理
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WAAA");
            _isPlayerInRange = true; // プレイヤーが範囲内にいることをフラグで管理

        }
    }
    private void Update()
    {
        //debug
#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    objectFallController.TriggerFall();
        //}
#endif
    }
    //トリガーからプレイヤーが出たときの処理
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false; // プレイヤーが範囲外に出たことをフラグで管理
            // 自分自身を無効化
            //DisableTrigger();
        }
    }

    //private void DisableTrigger()
    //{ 
    //    gameObject.SetActive(false);
    //}
    private void PushTheBrige()
    {
        if (_isPlayerInRange)
        {
            objectFallController.TriggerFall();
            GetComponent<BoxCollider>().enabled = false;
        }
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