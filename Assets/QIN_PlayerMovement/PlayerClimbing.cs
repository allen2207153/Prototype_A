using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing:MonoBehaviour
{
    // private float _climbHeight = 3f; // クライムの高さ (未使用)
    private float _lowClimbHight = 0.7f; // クライムの低い位置
    private float _bodyHight = 1.4f; // プレイヤーの身体の高さ
    private float _checkDistance = 1f; // レイキャストのチェック距離
    private Vector3 _climbHitNormal; // クライム時にヒットした面の法線
    public float climbAngle = 45f; 

    /// <summary>
    /// クライム検測
    /// </summary>
    /// <param name="playerTransform">プレイヤの位置</param>
    /// <param name="playerInput">プレイヤの現在の入力方向</param>
    /// <param name="climbPos">クライムの座標</param>
    /// <returns>bool（クライムの可否）</returns>
    /// 
    private void Start()
    {
        _checkDistance = Mathf.Cos(climbAngle) * _checkDistance; 
    }
    public bool ClimbDetect(Transform playerTransform, Vector3 playerInput, out Vector3 climbPos)
    {
        climbPos = default(Vector3);
        Vector3 rayStart = playerTransform.position + Vector3.up * _lowClimbHight;
        Vector3 rayDirection = playerTransform.forward;

        Debug.DrawRay(rayStart, rayDirection * _checkDistance, Color.red);

        // 障害物にヒットしたか？
        if (!Physics.Raycast(rayStart, rayDirection, out RaycastHit obsHit, _checkDistance))
        {
            return false;
        }

        _climbHitNormal = obsHit.normal; // ヒットした面の法線を取得

        // クライム角度の判定をここで行う（初期化後）
        if (Vector3.Angle(-_climbHitNormal, playerTransform.forward) > climbAngle ||
            Vector3.Angle(-_climbHitNormal, playerInput) > climbAngle)
        {
            return false;
        }

        // 段階的に壁をチェック
        for (int i = 0; i < 3; i++)
        {
            Vector3 checkPos = playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * i);

            if (!Physics.Raycast(checkPos, -_climbHitNormal, out RaycastHit wallHit, _checkDistance))
            {
                continue; // 次の段階へ
            }

            Debug.DrawRay(checkPos, -_climbHitNormal * _checkDistance, Color.red);

            // 上方向に Raycast を打ち、足場の有無を確認
            Vector3 ledgeCheckPos = wallHit.point + Vector3.up * _bodyHight;

            if (Physics.Raycast(ledgeCheckPos, Vector3.down, out RaycastHit ledgeHit, _bodyHight))
            {
                Debug.DrawRay(ledgeCheckPos, Vector3.down * _bodyHight, Color.green); // ヒット確認用
                climbPos = ledgeHit.point;
                return true;
            }
        }

        return false;
    }



}
