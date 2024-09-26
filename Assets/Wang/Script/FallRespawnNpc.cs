using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FallRespawnNpc : MonoBehaviour
{
    public Transform playerRespawnPoint;  // プレイヤーのリスポーンポイント
    public Transform npcRespawnPoint;     // NPCのリスポーンポイント
    public float fadeDuration = 1.0f;     // フェードイン・フェードアウトの時間

    private CharacterController playerController;  // プレイヤーのコントローラー
    private CharacterController npcController;     // NPCのコントローラー

    private void Start()
    {
        // ゲーム開始時にプレイヤーとNPCのコントローラーを検索
        playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        npcController = GameObject.FindWithTag("imouto").GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーかNPCがトリガーに触れた場合、両方をリスポーンさせる
        if (other.CompareTag("Player") || other.CompareTag("imouto"))
        {
            if (playerController != null && npcController != null)
            {
                StartCoroutine(RespawnBoth(playerController, npcController, playerRespawnPoint, npcRespawnPoint));
            }
        }
    }

    private IEnumerator RespawnBoth(CharacterController player, CharacterController npc, Transform playerRespawnPoint, Transform npcRespawnPoint)
    {
        // コントローラーを無効化
        FadeCanvas.Instance.FadeIn();
        yield return new WaitForSeconds(fadeDuration);
        player.enabled = false;
        npc.enabled = false;

      
                        // プレイヤーの新しいリスポーン位置を設定
                        player.transform.position = playerRespawnPoint.position;
                        player.transform.rotation = playerRespawnPoint.rotation;

                        // NPCの新しいリスポーン位置を設定
                        npc.transform.position = npcRespawnPoint.position;
                        npc.transform.rotation = npcRespawnPoint.rotation;

                        // フェードアウトの前にコントローラーを再有効化
                        player.enabled = true;
                        npc.enabled = true;
        FadeCanvas.Instance.FadeOut();
        yield return new WaitForSeconds(fadeDuration);

    }
}