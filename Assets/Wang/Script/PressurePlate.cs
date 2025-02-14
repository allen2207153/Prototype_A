using System.Collections;
using UnityEngine;

// 感圧地板の抽象クラス。基本の属性と動作を定義
public abstract class PressurePlate : MonoBehaviour
{
    public Vector3 sinkDistance = new Vector3(0, -0.2f, 0); // 沈む距離
    public float sinkDuration = 0.5f; // 沈むアニメーション時間
    public float returnDuration = 0.5f; // 元に戻るアニメーション時間

    private Vector3 initialPosition; // 初期位置
    protected bool isActivated = false; // アクティブ状態
    public DoorOpenTrigger doorOpenTrigger; // ドアの開閉を制御するトリガー

    protected virtual void Start()
    {
        initialPosition = transform.position;
    }

    // 抽象メソッド。具体的な感圧条件は派生クラスで実装
    public abstract void Activate();
    public abstract void Deactivate();

    // 下沈アニメーションのコルーチン
    protected IEnumerator Sink()
    {
        Vector3 targetPosition = initialPosition + sinkDistance;
        float elapsedTime = 0;

        while (elapsedTime < sinkDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       
        transform.position = targetPosition;
      
            doorOpenTrigger.OpenDoor();
            Debug.Log("Successsssssssssssss");
        

    }

    // 初期位置に戻るコルーチン
    protected IEnumerator ReturnToInitialPosition()
    {
        Vector3 currentPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < returnDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, initialPosition, elapsedTime / returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;

        

            doorOpenTrigger.CloseDoor();
       
    }
}