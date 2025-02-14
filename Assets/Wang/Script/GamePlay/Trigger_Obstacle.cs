using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trigger_Obstacle : MonoBehaviour
{
    public ObjectFallController objectFallController; // オブジェクト落下のコントローラー
    private bool _isPlayerInRange = false;               // プレイヤーが範囲内にいるかどうかを判定するフラグ
    public GameObject hintController; // ButtonHintControllerを持つGameObject
    private ButtonHintController buttonHintController; // ButtonHintControllerへの参照
    private BoxCollider hintTrigger;

    private void Start()
    {
        // ButtonHintControllerとBoxColliderを取得
        if (hintController != null)
        {
            hintTrigger = hintController.GetComponent<BoxCollider>();
            buttonHintController = hintController.GetComponent<ButtonHintController>();
        }
        else
        {
            Debug.LogError("HintController is not assigned.");
        }
    }

    private void OnEnable()
    {
        EventSystem.Instance.StartListening(GameEvents.PushTheBrige, PushTheBrige);
    }

    private void OnDisable()
    {
        EventSystem.Instance.StopListening(GameEvents.PushTheBrige, PushTheBrige);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WAAA");
            _isPlayerInRange = true; // プレイヤーが範囲内にいることをフラグで管理
            buttonHintController?.SetButtonPrompt(true); // UIを表示
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false; // プレイヤーが範囲外に出たことをフラグで管理
            buttonHintController?.SetButtonPrompt(false); // UIを非表示
        }
    }

    private void PushTheBrige()
    {
        if (_isPlayerInRange)
        {
            objectFallController.TriggerFall(); // オブジェクトを落下させる
            GetComponent<BoxCollider>().enabled = false; // 現在のトリガーを無効化
            if (hintTrigger != null)
            {
                hintTrigger.enabled = false; // Hint UI のトリガーを無効化
            }
            buttonHintController?.SetButtonPrompt(false); // UIを非表示
        }
    }

    private void OnDrawGizmos()
    {
        var boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
