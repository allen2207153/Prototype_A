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

    public float movableObjectHeight = 0.8f;
    public LayerMask _movableLayer;

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
        climbPos = default(Vector3); // クライム位置の初期化
        Vector3 rayStart = playerTransform.position + Vector3.up * _lowClimbHight; // レイキャストの開始位置
        Vector3 rayDirection = playerTransform.forward; // レイキャストの方向

        Debug.DrawRay(rayStart, rayDirection * _checkDistance, Color.red); // デバッグ用のレイを描画

        if (Vector3.Angle(-_climbHitNormal, playerTransform.forward) > climbAngle || Vector3.Angle(-_climbHitNormal, playerInput) > climbAngle)
        {
            //Debug.Log("角度不正");
            return false;
        }
        

        // 障害物にヒットしたかどうかをチェック
        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit obsHit, _checkDistance))
        {
            _climbHitNormal = obsHit.normal; // ヒットした面の法線を取得
            // プレイヤーの前方方向とヒット面の法線の角度をチェック
            if (Vector3.Angle(-_climbHitNormal, playerTransform.forward) > 45
                || Vector3.Angle(-_climbHitNormal, playerInput) > 45)
            {
                return false; // クライム不可
            }

            // 最初の壁にヒットしたかどうかをチェック
            if (Physics.Raycast(playerTransform.position + Vector3.up * _lowClimbHight
                , -_climbHitNormal
                , out RaycastHit firstWallHit
                , _checkDistance))
            {
                Debug.DrawRay(playerTransform.position + Vector3.up * _lowClimbHight
                    , -_climbHitNormal * _checkDistance
                    , Color.red); // デバッグ用のレイを描画

                // 2番目の壁にヒットしたかどうかをチェック
                if (Physics.Raycast(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight)
                    , -_climbHitNormal
                    , out RaycastHit secondWallHit
                    , _checkDistance))
                {
                    Debug.DrawRay(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight)
                        , -_climbHitNormal * _checkDistance
                        , Color.red); // デバッグ用のレイを描画

                    // 3番目の壁にヒットしたかどうかをチェック
                    if (Physics.Raycast(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 2)
                        , -_climbHitNormal
                        , out RaycastHit thirdWallHit
                        , _checkDistance))
                    {
                        Debug.DrawRay(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 2)
                            , -_climbHitNormal * _checkDistance
                            , Color.red); // デバッグ用のレイを描画

                        // 4番目の壁にヒットしたかどうかをチェック
                        if (Physics.Raycast(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 3)
                            , -_climbHitNormal
                            , _checkDistance))
                        {
                            Debug.DrawRay(playerTransform.position + Vector3.up * (_lowClimbHight + _bodyHight * 3)
                                , -_climbHitNormal * _checkDistance
                                , Color.red); // デバッグ用のレイを描画

                            return false; // クライム不可
                        }
                        else if (Physics.Raycast(thirdWallHit.point + Vector3.up * _bodyHight
                            , Vector3.down
                            , out RaycastHit ledgeHit
                            , _bodyHight))
                        {
                            Debug.DrawRay(thirdWallHit.point + Vector3.up * _bodyHight
                                , Vector3.down * _bodyHight
                                , Color.red); // デバッグ用のレイを描画

                            climbPos = ledgeHit.point; // クライム位置を設定
                            return true; // クライム可能
                        }
                    }
                    else if (Physics.Raycast(secondWallHit.point + Vector3.up * _bodyHight
                            , Vector3.down
                            , out RaycastHit ledgeHit
                            , _bodyHight))
                    {
                        Debug.DrawRay(secondWallHit.point + Vector3.up * _bodyHight
                                , Vector3.down * _bodyHight
                                , Color.red); // デバッグ用のレイを描画

                        climbPos = ledgeHit.point; // クライム位置を設定
                        return true; // クライム可能
                    }
                }
                else if (Physics.Raycast(firstWallHit.point + Vector3.up * _bodyHight
                            , Vector3.down
                            , out RaycastHit ledgeHit
                            , _bodyHight))
                {
                    Debug.DrawRay(firstWallHit.point + Vector3.up * _bodyHight
                                , Vector3.down * _bodyHight
                                , Color.red); // デバッグ用のレイを描画

                    climbPos = ledgeHit.point; // クライム位置を設定
                    return true; // クライム可能
                }
            }
        }
        return false; // クライム不可
    }

    public MovableObject MovableObjectCheck(Transform playerTransform, Vector3 inputDirection)
    {
        if (Physics.Raycast(playerTransform.position + Vector3.up * movableObjectHeight, playerTransform.forward, out RaycastHit hit, _checkDistance, _movableLayer))
        {
            _climbHitNormal = hit.normal;
            if (Vector3.Angle(-_climbHitNormal, playerTransform.forward) > climbAngle || Vector3.Angle(-_climbHitNormal, inputDirection) > climbAngle)
            {
                return null;
            }
            MovableObject movableObject;
            if (hit.collider.TryGetComponent<MovableObject>(out movableObject))
            {
                return movableObject;
            }
        }
        return null;
    }
    
}
