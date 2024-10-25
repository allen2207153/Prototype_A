
using System.Collections;
using UnityEngine;

public class PlayerRespawnController : MonoBehaviour
{
    // プレイヤーのコントローラー
    private CharacterController _playerController;

    // NPCのコントローラー（妹）
    private CharacterController _imoutoController;

    // フェードの継続時間
    public float FadeDuration = 1.0f;

    [Header("観測用")]
    // プレイヤーのリスポーン位置
    [SerializeField] private Vector3 _playerRespawnPoint = default;

    // 妹のリスポーン位置
    [SerializeField] private Vector3 _imoutoRespawnPoint = default;

    // イベントを登録
    private void OnEnable()
    {
        PlayerEvent.PlayerRespawn += PlayerRespawn;
        PlayerEvent.UpdateRespawnPoint += UpdateRespawnPoint;
    }

    void Start()
    {
        // ゲーム開始時にプレイヤーと妹のコントローラーを検索
        _playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        _imoutoController = GameObject.FindWithTag("imouto").GetComponent<CharacterController>();
        UpdateRespawnPoint();
    }

    private void UpdateRespawnPoint()
    {
        if (_playerController != null && _imoutoController != null)
        {
            _playerRespawnPoint = _playerController.transform.position;
            _imoutoRespawnPoint = _imoutoController.transform.position;
            Debug.Log("UpdateRespawnPoint");
        }
    }

    private void PlayerRespawn()
    {
        if (_playerController != null && _imoutoController != null)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    // リスポーン処理をコルーチンで実行する
    private IEnumerator RespawnCoroutine()
    {
        // コントローラーを無効化
        FadeCanvas.Instance.FadeIn();
        yield return new WaitForSeconds(FadeDuration);
        _playerController.enabled = false;
        _imoutoController.enabled = false;

        // プレイヤーの新しいリスポーン位置を設定
        _playerController.transform.position = _playerRespawnPoint;

        // 妹の新しいリスポーン位置を設定
        _imoutoController.transform.position = _imoutoRespawnPoint;

        // コントローラーを再有効化する前にフェードアウト
        _playerController.enabled = true;
        _imoutoController.enabled = true;
        yield return new WaitForSeconds(FadeDuration + 0.5f);
        FadeCanvas.Instance.FadeOut();
    }

    private void OnDisable()
    {
        PlayerEvent.PlayerRespawn -= PlayerRespawn;
        PlayerEvent.UpdateRespawnPoint -= UpdateRespawnPoint;
    }
}
