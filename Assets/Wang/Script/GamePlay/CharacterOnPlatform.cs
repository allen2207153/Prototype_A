using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnPlatform : MonoBehaviour
{
    private CharacterController characterController;
    private Transform platformTransform;
    private Vector3 previousPlatformPosition;
    private bool isOnPlatform;

    void Start()
    {
        characterController = GetComponent<CharacterController>(); // コンポーネント取得
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Platform")) // プラットフォームに接触
        {
            if (!isOnPlatform) // 初めて接触時に位置を記録
            {
                platformTransform = hit.transform;
                previousPlatformPosition = platformTransform.position;
                isOnPlatform = true;
            }
        }
    }

    void Update()
    {
        if (isOnPlatform && platformTransform != null) // プラットフォーム上にいる場合
        {
            Vector3 platformMovement = platformTransform.position - previousPlatformPosition; // 移動量計算
            characterController.Move(platformMovement); // 移動を適用
            previousPlatformPosition = platformTransform.position; // 位置更新

            if (!characterController.isGrounded) isOnPlatform = false; // 接地していなければフラグをオフ
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform")) // プラットフォームから離れたらフラグをリセット
        {
            isOnPlatform = false;
            platformTransform = null;
        }
    }
}

