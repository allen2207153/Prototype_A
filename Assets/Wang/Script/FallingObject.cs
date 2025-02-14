using System.Collections;
using UnityEngine;

/// <summary>
/// 落石や倒壊する柱を管理する汎用クラス
/// </summary>
public class FallingObject : MonoBehaviour
{
    [Header("落下・倒壊の設定")]
    [SerializeField] private float triggerDistance = 5f; // プレイヤーとの距離がこの値以内になると落下
    [SerializeField] private float delayBeforeFall = 0.5f; // 落下までの遅延時間
    [SerializeField] private bool useTrigger = false; // トリガーで落下する場合 true

    [Header("物理設定")]
    [SerializeField] private Rigidbody rb; // 物理挙動を制御する Rigidbody
    [SerializeField] private Vector3 fallForce = new Vector3(0, -10f, 0); // 落下時に加える力

    private bool hasFallen = false; // すでに落下したかのフラグ
    private Transform player; // プレイヤーの Transform

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>(); // Rigidbody がアタッチされていない場合、自動取得
        }

        if (rb != null)
        {
            rb.isKinematic = true; // 初期状態では物理挙動を無効化
        }

        // プレイヤーオブジェクトをシーン内から探す（"Player" タグを持つオブジェクトを想定）
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        // すでに落下している場合、処理しない
        if (hasFallen) return;

        // プレイヤーが一定距離内に入った場合、落下を開始
        if (player != null && Vector3.Distance(transform.position, player.position) <= triggerDistance)
        {
            StartCoroutine(StartFalling());
        }
    }

    /// <summary>
    /// 一定時間後に落下処理を開始するコルーチン
    /// </summary>
    private IEnumerator StartFalling()
    {
        hasFallen = true; // 落下フラグを設定
        yield return new WaitForSeconds(delayBeforeFall); // 指定時間待機

        if (rb != null)
        {
            rb.isKinematic = false; // 物理挙動を有効化
            rb.AddForce(fallForce, ForceMode.Impulse); // 物理的な力を加える（オブジェクトに動きを与える）
        }
    }

    /// <summary>
    /// トリガーに当たった場合に落下を開始する（トリガーモード時のみ）
    /// </summary>
    /// <param name="other">衝突したオブジェクトのコライダー</param>
    private void OnTriggerEnter(Collider other)
    {
        // トリガーモードが有効であり、プレイヤーがトリガーに入った場合に落下
        if (useTrigger && other.CompareTag("Player"))
        {
            StartCoroutine(StartFalling());
        }
    }
}
