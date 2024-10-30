using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenTrigger : MonoBehaviour
{
    private bool BoardTrigger;
    public GameObject doorObject;          // 開くドアのオブジェクト
    public float openHeight = 3.0f;        // ドアが上に開く高さ
    public float openDuration = 1.0f;      // ドアが開くまでの時間
    public float closeDuration = 1.0f;     // ドアが閉まるまでの時間

    private Vector3 initialDoorPosition;   // ドアの初期位置
    private Vector3 targetPosition;        // ドアが完全に開いた時の位置
    private bool isDoorOpen = false;       // ドアが開いているかどうか

    void Start()
    {
        // ドアの初期位置を記録
        if (doorObject != null)
        {
            initialDoorPosition = doorObject.transform.position;
            targetPosition = initialDoorPosition + new Vector3(0, openHeight, 0); // ドアが開く目標位置
        }
        else
        {
            Debug.LogError("Door object is not assigned!");
        }

      
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    public void CloseDoor()
    {
        StartCoroutine(CloseDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        float elapsedTime = 0;

        // ドアが指定した高さまで上昇する
        while (elapsedTime < openDuration)
        {
            doorObject.transform.position = Vector3.Lerp(initialDoorPosition, targetPosition, elapsedTime / openDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorObject.transform.position = targetPosition;
    }

    private IEnumerator CloseDoorCoroutine()
    {
        float elapsedTime = 0;

        // ドアが元の位置に戻る
        while (elapsedTime < closeDuration)
        {
            doorObject.transform.position = Vector3.Lerp(targetPosition, initialDoorPosition, elapsedTime / closeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorObject.transform.position = initialDoorPosition;
    }
}
