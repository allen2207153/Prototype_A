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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MovableObject MovableObjectCheck(Transform playerTransform)
    {
        // プレイヤーの足元から、3点でRaycastを発射
        Vector3[] raycastPositions = new Vector3[]
        {
        playerTransform.position + Vector3.up * _movableObjectHeight, // 中央
        playerTransform.position + Vector3.up * _movableObjectHeight + playerTransform.right * 0.5f, // 右側
        playerTransform.position + Vector3.up * _movableObjectHeight - playerTransform.right * 0.5f  // 左側
        };

        foreach (var rayStart in raycastPositions)
        {
            // 各位置からRaycastを発射
            if (Physics.Raycast(rayStart, playerTransform.forward, out RaycastHit hit, _checkDistance, _movableLayer))
            {
                _pushHitNormal = hit.normal;

                // プッシュ角度の条件を満たしているか確認
                if (Vector3.Angle(-_pushHitNormal, playerTransform.forward) > _pushAngle)
                {
                    continue;
                }

                // ヒットしたオブジェクトがMovableObjectかどうかチェック
                if (hit.collider.TryGetComponent<MovableObject>(out MovableObject movableObject))
                {
                    return movableObject;
                }
            }
        }

        // いずれのRaycastでも検出されなければnullを返す
        return null;
    }

    }
