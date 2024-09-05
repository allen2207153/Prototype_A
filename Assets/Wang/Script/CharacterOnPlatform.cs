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
        characterController = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 检查角色是否站在带有“Platform”标签的物体上
        if (hit.gameObject.CompareTag("Platform"))
        {
            // 记录平台Transform
            if (!isOnPlatform)
            {
                platformTransform = hit.transform;
                previousPlatformPosition = platformTransform.position;
                isOnPlatform = true;
            }
        }
    }

    void Update()
    {
        if (isOnPlatform && platformTransform != null)
        {
            // 计算平台的移动量
            Vector3 platformMovement = platformTransform.position - previousPlatformPosition;

            // 将平台的移动量应用到角色上
            characterController.Move(platformMovement);

            // 更新平台的前一次位置
            previousPlatformPosition = platformTransform.position;

            // 检查角色是否仍在平台上
            if (!characterController.isGrounded)
            {
                isOnPlatform = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 当角色离开平台时，重置状态
        if (other.CompareTag("Platform"))
        {
            isOnPlatform = false;
            platformTransform = null;
        }
    }
}

