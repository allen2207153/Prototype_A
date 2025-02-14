using DG.Tweening;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class SwitchStage : MonoBehaviour
{
    public Transform playerRespawnPoint;  // プレイヤーのリスポーンポイント
    public Transform npcRespawnPoint;     // NPCのリスポーンポイント
    public float fadeDuration = 1.0f;     // フェードイン・フェードアウトの時間
    //public CinemachineVirtualCamera newCamera; // 切り替え後のCinemachineカメラ

   [SerializeField] private CharacterController playerController;
    private CharacterController npcController;
    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        // プレイヤーとNPCのコントローラーを取得
       //playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        npcController = GameObject.FindWithTag("imouto").GetComponent<CharacterController>();
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // NPCがトリガーに触れたらリスポーン処理を開始
        if (other.CompareTag("imouto")|| other.CompareTag("Player"))
        {
            if (playerController != null && npcController != null)
            {
                StartCoroutine(RespawnBoth(playerController, npcController, playerRespawnPoint, npcRespawnPoint));
            }
        }
    }

    private IEnumerator RespawnBoth(CharacterController player, CharacterController npc, Transform playerRespawnPoint, Transform npcRespawnPoint)
    {
        // フェードイン開始
        FadeCanvas.Instance.FadeIn();
        yield return new WaitForSeconds(fadeDuration * 0.5f); // フェードの半分の時間待機

        // コントローラーを無効化
        player.enabled = false;
        npc.enabled = false;

        // プレイヤーの新しいリスポーン位置を設定
        player.transform.position = playerRespawnPoint.position;
        player.transform.rotation = playerRespawnPoint.rotation;

        // NPCの新しいリスポーン位置を設定
        npc.transform.position = npcRespawnPoint.position;
        npc.transform.rotation = npcRespawnPoint.rotation;

        yield return new WaitForSeconds(fadeDuration * 0.5f); // 残りのフェードイン時間待機

        // カメラを切り替え
        //if (newCamera != null)
        //{
        //    newCamera.Priority = 100; // 優先度を上げてカメラを切り替え
        //}

        // コントローラーを再有効化
        player.enabled = true;
        npc.enabled = true;

        // フェードアウト開始
        FadeCanvas.Instance.FadeOut();
        yield return new WaitForSeconds(fadeDuration);
    }
}
