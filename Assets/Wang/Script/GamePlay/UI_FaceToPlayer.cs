using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FaceToPlayer : MonoBehaviour
{
    private Transform playerCamera;

    void Start()
    {
        // プレイヤーのカメラを取得
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        // LookAt を使用して、UI がカメラに向くようにする
        transform.LookAt(playerCamera);
        transform.rotation = Quaternion.LookRotation(playerCamera.forward);
    }
}
