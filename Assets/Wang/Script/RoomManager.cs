using System.Collections;
using UnityEngine;

/// <summary>
/// RoomManagerクラス：プレイヤーやNPCのリスポーン処理、敵のリセット、
/// dissolveアニメーション、フェード効果を管理する。
/// </summary>
public class RoomManager : MonoBehaviour
{
    public Transform playerRespawnPoint; // プレイヤーのリスポーン位置
    public Transform npcRespawnPoint;    // NPCのリスポーン位置
    public EnemySpawnTrigger enemySpawnTrigger; // 敵のスポーンを管理するトリガー
    public float fadeDuration = 1.0f;    // フェードイン・フェードアウトの時間
    public float dissolveDuration = 2.0f; // dissolveアニメーションの時間

    private bool isTriggered = false; // 処理の多重実行を防止するフラグ

    /// <summary>
    /// 外部から呼び出されるリセット処理を開始する。
    /// </summary>
    /// <param name="player">リスポーン対象のプレイヤー</param>
    /// <param name="npc">リスポーン対象のNPC</param>
    public void TriggerReset(GameObject player, GameObject npc)
    {
        if (isTriggered) return; // 既に処理中の場合は何もしない

        StartCoroutine(HandleReset(player, npc)); // リセット処理を開始
    }

    /// <summary>
    /// リセット処理のコルーチン。
    /// dissolveアニメーション、フェードイン・フェードアウト、リスポーンを実行する。
    /// </summary>
    private IEnumerator HandleReset(GameObject player, GameObject npc)
    {
        isTriggered = true; // 処理中フラグを立てる
        var npcAnimator = npc.GetComponent<Animator>(); // NPCのアニメーションコントローラーを取得

        // NPCのdissolveアニメーションを実行
        if (npc != null)
        {
            var npcMovement = npc.GetComponent<PlayerMovement>();
            if (npcMovement != null)
            {
                npcAnimator.SetBool("isDead", true); // 死亡アニメーションを開始
                npc.GetComponent<CharacterController>().enabled = false; // NPCの操作を無効化
                npcMovement.dissolve(); // dissolveアニメーションを再生
                yield return new WaitForSeconds(dissolveDuration); // アニメーションの完了を待つ
            }
        }

        // フェードインを開始
        FadeCanvas.Instance.FadeIn();
        npcAnimator.SetBool("isDead", false); // 死亡アニメーションを終了
        yield return new WaitForSeconds(fadeDuration); // フェードインの完了を待つ

        // プレイヤーとNPCの位置をリセットし、敵のスポーン状態をリセット
        enemySpawnTrigger.ResetAllSpawn();
        ResetPlayerAndNPC(player, npc);

        // フェードアウトを開始
        FadeCanvas.Instance.FadeOut();
        yield return new WaitForSeconds(fadeDuration); // フェードアウトの完了を待つ

        isTriggered = false; // 処理中フラグをリセット
    }

    /// <summary>
    /// プレイヤーとNPCの位置をリスポーンポイントにリセットする。
    /// </summary>
    private void ResetPlayerAndNPC(GameObject player, GameObject npc)
    {
        // プレイヤーの位置をリセット
        if (player != null)
        {
            player.GetComponent<CharacterController>().enabled = false; // プレイヤー操作を無効化
            player.transform.position = playerRespawnPoint.position; // リスポーン位置に移動
            player.transform.rotation = playerRespawnPoint.rotation; // 回転もリセット
            player.GetComponent<PlayerMovement>()?.Reset(); // プレイヤーの動作状態をリセット
            player.GetComponent<CharacterController>().enabled = true; // プレイヤー操作を有効化
        }

        // NPCの位置をリセット
        if (npc != null)
        {
            npc.GetComponent<CharacterController>().enabled = false; // NPC操作を無効化
            npc.transform.position = npcRespawnPoint.position; // リスポーン位置に移動
            npc.transform.rotation = npcRespawnPoint.rotation; // 回転もリセット
            npc.GetComponent<PlayerMovement>()?.Reset(); // NPCの動作状態をリセット
            npc.GetComponent<CharacterController>().enabled = true; // NPC操作を有効化
        }
    }
}
