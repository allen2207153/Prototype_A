using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKOffset : MonoBehaviour
{
    public Transform player;  // 玩家物件
    public float sideOffset = 0.5f;  // NPC 偏移到玩家右側的距離
    public float followDistance = 1.5f;  // NPC 跟隨玩家的距離
    public float moveSpeed = 5.0f;  // NPC 移動速度
    public float minimumDistance = 0.5f;  // 與玩家的最小距離，避免過近

    private Vector3 targetPosition;  // 目標位置

    void Update()
    {
        // 計算NPC應該站在的位置，偏移到玩家的右側
        Vector3 offsetPosition = player.position + (player.right * sideOffset) - (player.forward * followDistance);

        // 計算與玩家的距離，避免距離過近
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < minimumDistance)
        {
            // 如果距離過近，調整位置遠離玩家
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            offsetPosition += directionAwayFromPlayer * (minimumDistance - distanceToPlayer);
        }

        // 平滑移動NPC到計算出的目標位置
        targetPosition = Vector3.Lerp(transform.position, offsetPosition, Time.deltaTime * moveSpeed);
        transform.position = targetPosition;

        // 讓NPC看向玩家
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}

