using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public LayerMask _movableLayer;

    [Header("箱が検出できる距離")]
    public float _checkDistance = 1f;

    public float _pushAngle = 45f;
    public Vector3 _pushHitNormal;

    [Header("プレイヤが押す箱の最低高さ")]
    public float _movableObjectHeight = 0.5f;

    // Raycast の位置を保存するための配列
    private Vector3[] raycastPositions;
    Vector3 obstacleHitNormal;

    void Start()
    {
        // Raycast の位置をプレイヤーの足元から設定
        raycastPositions = new Vector3[]
        {
            transform.position + Vector3.up * _movableObjectHeight, // 中央
            transform.position + Vector3.up * _movableObjectHeight + transform.right * 0.5f, // 右側
            transform.position + Vector3.up * _movableObjectHeight - transform.right * 0.5f  // 左側
        };
    }

    void Update()
    {
        // プレイヤーの位置に基づいて Raycast の位置を更新
        raycastPositions[0] = transform.position + Vector3.up * _movableObjectHeight;
        raycastPositions[1] = transform.position + Vector3.up * _movableObjectHeight + transform.right * 0.5f;
        raycastPositions[2] = transform.position + Vector3.up * _movableObjectHeight - transform.right * 0.5f;
    }

    //public MovableObject MovableObjectCheck(Transform playerTransform, Vector3 inputDirection)
    //{
    //    if (Physics.Raycast(playerTransform.position + Vector3.up * movableObjectHeight, playerTransform.forward, out RaycastHit hit, checkDistance, movableLayer))
    //    {
    //        climbHitNormal = hit.normal;
    //        if (Vector3.Angle(-climbHitNormal, playerTransform.forward) > climbAngle || Vector3.Angle(-climbHitNormal, inputDirection) > climbAngle)
    //        {
    //            return null;
    //        }
    //        MovableObject movableObject;
    //        if (hit.collider.TryGetComponent<MovableObject>(out movableObject))
    //        {
    //            return movableObject;
    //        }
    //    }
    //    return null;
    //}
    // Gizmos を使用して Raycast の可視化
    private void OnDrawGizmos()
    {
        if (raycastPositions == null) return;

        // Raycast の色と可視化範囲
        Gizmos.color = Color.yellow;
        foreach (var rayStart in raycastPositions)
        {
            Gizmos.DrawLine(rayStart, rayStart + transform.forward * _checkDistance);
        }
    }

}
