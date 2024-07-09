using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAction : MonoBehaviour
{
    public float longPressDuration = 1.0f; // 持續時間的閾值
    private float pressTime = 0.0f;
    private bool isPressing = false;

    void Update()
    {
        // 檢測K鍵按下
        if (Input.GetKeyDown(KeyCode.K))
        {
            isPressing = true;
            pressTime = 0.0f; // 重置計時器
        }

        // 檢測K鍵釋放
        if (Input.GetKeyUp(KeyCode.K))
        {
            isPressing = false;
        }

        // 計算按下時間
        if (isPressing)
        {
            pressTime += Time.deltaTime;

            if (pressTime >= longPressDuration)
            {
                PerformLongPressAction();
                isPressing = false; // 重置狀態
            }
        }
    }

    void PerformLongPressAction()
    {
        // 在這裡執行你想要的動作
        Debug.Log("Long press action performed!");
    }
}
