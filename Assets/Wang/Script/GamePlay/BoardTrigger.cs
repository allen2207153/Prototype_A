using System.Collections;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    public Vector3 sinkDistance = new Vector3(0, -0.2f, 0); // 板が下に沈む距離
    public float sinkDuration = 0.5f; // 沈むアニメーションの時間
    public Color glowColor = Color.yellow; // 板の発光色
    public float glowIntensity = 2.0f; // 発光の強さ
    public float blinkInterval = 0.5f; // 発光が点滅する間隔
    public float glowDuration = 2.0f; // プレイヤーが踏んだ後の発光が続く時間

    private Vector3 initialPosition; // 板の初期位置
    private Material boardMaterial; // 板のマテリアル
    private bool isActivated = false; // プレイヤーが踏んだかどうか
    private bool isGlowing = true; // 点滅状態を管理

    void Start()
    {
        // 板の初期位置を記録
        initialPosition = transform.position;

        // 板のマテリアルを取得（エミッション機能が必要）
        boardMaterial = GetComponent<Renderer>().material;

        // エミッションを有効化して点滅を開始
        StartCoroutine(BlinkEffect());
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに触れた場合
        if (!isActivated && other.CompareTag("imouto"))
        {
            isActivated = true;
            isGlowing = false; // 点滅を停止する
            StopCoroutine(BlinkEffect()); // 点滅のコルーチンを停止
            StartCoroutine(SinkAndGlow());
        }
    }

    IEnumerator SinkAndGlow()
    {
        // 板が沈むアニメーションを開始
        Vector3 targetPosition = initialPosition + sinkDistance;
        float elapsedTime = 0;

        while (elapsedTime < sinkDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // 板が発光し続ける（もしくは別の処理）
        StartCoroutine(GlowEffect());
    }

    IEnumerator GlowEffect()
    {
        // エミッションをオンにして板を発光させる
        boardMaterial.EnableKeyword("_EMISSION");
        boardMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);

        // 指定された時間発光させる
        yield return new WaitForSeconds(glowDuration);

        // エミッションをオフにして元に戻す
        boardMaterial.DisableKeyword("_EMISSION");
    }

    IEnumerator BlinkEffect()
    {
        // 点滅し続ける処理
        while (isGlowing)
        {
            // 発光する
            boardMaterial.EnableKeyword("_EMISSION");
            boardMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);

            // 点滅間隔の半分待機
            yield return new WaitForSeconds(blinkInterval / 2);

            // 発光をオフにする
            boardMaterial.DisableKeyword("_EMISSION");

            // 点滅間隔の残り半分待機
            yield return new WaitForSeconds(blinkInterval / 2);
        }
    }
}