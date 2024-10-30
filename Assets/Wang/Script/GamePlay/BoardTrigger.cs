using System.Collections;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    public Vector3 sinkDistance = new Vector3(0, -0.2f, 0); // 板が下に沈む距離
    public float sinkDuration = 0.5f; // 沈むアニメーションの時間
    public float returnDuration = 0.5f; // 元の位置に戻るアニメーションの時間

    private Vector3 initialPosition; // 板の初期位置
    public bool isActivated = false; // プレイヤーが踏んだかどうか

    private DoorOpenTrigger doorOpenTrigger; // DoorOpenTriggerの参照
    private Collider triggerCollider; // トリガーのコライダー

    void Start()
    {
        // 板の初期位置を記録
        initialPosition = transform.position;

        // DoorOpenTriggerを取得
        doorOpenTrigger = GetComponent<DoorOpenTrigger>();

        // トリガーのコライダーを取得
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
        {
            Debug.LogError("Collider component is missing on the board object.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに触れた場合
        if (!isActivated && (other.CompareTag("Player")|| other.CompareTag("imouto")))
        {
            isActivated = true;
            StartCoroutine(SinkAndTriggerDoor());
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーがトリガーから離れた場合
        if (isActivated && (other.CompareTag("Player") || other.CompareTag("imouto")))
        {
            isActivated = false;
            StartCoroutine(ReturnToInitialPosition());
        }
    }

    IEnumerator SinkAndTriggerDoor()
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

        // ドアを開ける
        if (doorOpenTrigger != null)
        {
            doorOpenTrigger.OpenDoor();
        }
    }

    IEnumerator ReturnToInitialPosition()
    {
        // トリガーを無効化
        triggerCollider.enabled = false;

        // 元の位置に戻るアニメーション
        Vector3 currentPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < returnDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, initialPosition, elapsedTime / returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;

        // ドアを閉める
        if (doorOpenTrigger != null)
        {
            doorOpenTrigger.CloseDoor();
        }

        // トリガーを再度有効化
        triggerCollider.enabled = true;
    }
}