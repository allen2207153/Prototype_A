using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public Transform playerRespawnPoint;  // 玩家复活点的Transform
    public Transform npcRespawnPoint;     // NPC复活点的Transform
    public float fadeDuration = 1.0f;     // 淡入淡出的持续时间

    public FadeCanvas sharedFadeCanvas;   // 共享的FadeCanvas，或为每个对象独立创建一个FadeCanvas

    private void OnTriggerEnter(Collider other)
    {
        // 检查是玩家还是NPC
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null)
        {
            // 判断是否是玩家（可以通过Tag或其他方式区分）
            if (other.CompareTag("Player")||other.CompareTag("imouto"))
            {
                Respawn(controller, playerRespawnPoint);  // 玩家重生点
                Respawn(controller, npcRespawnPoint);  // NPC重生点
            }
            
        }
    }

    private void Respawn(CharacterController controller, Transform respawnPoint)
    {
        // 禁用角色控制器
        controller.enabled = false;

        // 共享的FadeCanvas进行淡入淡出处理
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.AppendCallback(() => sharedFadeCanvas.FadeIn())   // 淡入
                    .AppendInterval(fadeDuration)   // 等待淡入完成
                    .AppendCallback(() =>
                    {
                        // 设置新的重生位置
                        controller.transform.position = respawnPoint.position;
                        controller.transform.rotation = respawnPoint.rotation;

                        // 在淡出之前重新启用角色控制器
                        controller.enabled = true;
                        Debug.Log(controller.name + " 已重生于 " + respawnPoint.name);
                    })
                    .AppendCallback(() => sharedFadeCanvas.FadeOut())  // 执行淡出
                    .AppendInterval(fadeDuration);   // 等待淡出完成
    }
}
