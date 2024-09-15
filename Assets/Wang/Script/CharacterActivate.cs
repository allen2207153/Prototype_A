using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActivate : MonoBehaviour
{
    public GameObject targetObject; // CharacterControllerとAnimatorを持つオブジェクト
    public Transform resetTransform; // リセット先の空のゲームオブジェクト

    private CharacterController characterController; // キャラクターコントローラー
    private Animator animator; // アニメーター

    void Start()
    {
        // targetObjectからCharacterControllerとAnimatorを取得
        if (targetObject != null)
        {
            characterController = targetObject.GetComponent<CharacterController>();
            animator = targetObject.GetComponent<Animator>();

            // 最初は無効にしておく
            if (characterController != null)
            {
                characterController.enabled = false;
            }
            if (animator != null)
            {
                animator.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // トリガーに触れたオブジェクトがプレイヤーの場合
        if (other.CompareTag("Player"))
        {
            // 指定された空のオブジェクトの位置と回転にリセット
            if (targetObject != null && resetTransform != null)
            {
                targetObject.transform.position = resetTransform.position;
                targetObject.transform.rotation = resetTransform.rotation;
            }

            // CharacterControllerとAnimatorを有効にする
            if (characterController != null)
            {
                characterController.enabled = true;
            }
            if (animator != null)
            {
                animator.enabled = true;
            }
        }
    }
}
