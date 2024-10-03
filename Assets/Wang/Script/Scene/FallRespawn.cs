using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public Transform playerRespawnPoint;  // 玩家复活点的Transform
    public Transform npcRespawnPoint;     // NPC复活点的Transform
    public float fadeDuration = 1.0f;     // 淡入淡出的持续时间

    private Animator animator; 
    private void OnTriggerEnter(Collider other)
    {
        // 检查是玩家还是NPC
        CharacterController controller = other.GetComponent<CharacterController>();

        if (controller != null)
        {
            // 判断是否是玩家（可以通过Tag或其他方式区分）
            if (other.CompareTag("Player")||other.CompareTag("imouto"))
            {
                StartCoroutine(Respawn(controller, playerRespawnPoint));  // 玩家重生点
                StartCoroutine(Respawn(controller, npcRespawnPoint));  // NPC重生点
            }
            
        }
    }

    private IEnumerator Respawn(CharacterController controller, Transform respawnPoint)
    {
        // 禁用角色控制器
        
        FadeCanvas.Instance.FadeIn();
        yield return new WaitForSeconds(fadeDuration);
        controller.enabled = false;
        // 设置新的重生位置
        controller.transform.position = respawnPoint.position;
        controller.transform.rotation = respawnPoint.rotation;
        // CharacterControllerとAnimatorを有効にする
        if (controller != null)
        {
            controller.enabled = true;
        }
        if (animator != null)
        {
            animator.enabled = true;
        }

        // フェードアウト
        FadeCanvas.Instance.FadeOut();
        yield return new WaitForSeconds(fadeDuration);

    }
}
