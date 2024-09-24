using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawnNpc : MonoBehaviour
{
    public Transform playerRespawnPoint;  // プレイヤーのリスポーンポイント
    public Transform npcRespawnPoint;     // NPCのリスポーンポイント
    public float fadeDuration = 1.0f;     // フェードイン・フェードアウトの時間

    public FadeCanvas sharedFadeCanvas;   // 共有するFadeCanvas、もしくは個別に作成したFadeCanvas

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
                RespawnBoth(playerController, npcController, playerRespawnPoint, npcRespawnPoint);
            }
        }
    }

    private void RespawnBoth(CharacterController player, CharacterController npc, Transform playerRespawnPoint, Transform npcRespawnPoint)
    {
        // コントローラーを無効化
        player.enabled = false;
        npc.enabled = false;

        // 共有のFadeCanvasを使ってフェードイン・フェードアウトを行う
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.AppendCallback(() => sharedFadeCanvas.FadeIn())   // フェードイン
                    .AppendInterval(fadeDuration)   // フェードイン完了まで待つ
                    .AppendCallback(() =>
                    {
                        // プレイヤーの新しいリスポーン位置を設定
                        player.transform.position = playerRespawnPoint.position;
                        player.transform.rotation = playerRespawnPoint.rotation;

                        // NPCの新しいリスポーン位置を設定
                        npc.transform.position = npcRespawnPoint.position;
                        npc.transform.rotation = npcRespawnPoint.rotation;

                        // フェードアウトの前にコントローラーを再有効化
                        player.enabled = true;
                        npc.enabled = true;

                        Debug.Log(player.name + " が " + playerRespawnPoint.name + " にリスポーンしました");
                        Debug.Log(npc.name + " が " + npcRespawnPoint.name + " にリスポーンしました");
                    })
                    .AppendCallback(() => sharedFadeCanvas.FadeOut())  // フェードアウト
                    .AppendInterval(fadeDuration);   // フェードアウト完了まで待つ
    }
}