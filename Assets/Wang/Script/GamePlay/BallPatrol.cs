using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPatrol : MonoBehaviour
{
    public Transform player;         // プレイヤーの位置
    public LayerMask obstructionMask; // 障害物のレイヤー
    public float detectionRange = 10f;  // 検知範囲
    public float detectionAngle = 45f;  // 検知角度
    public float validRange = 8f;       // 判定範囲、検知範囲より小さい

    public float patrolSpeed = 2f;      // 巡回速度
    public float patrolMinAngleY = -45f; // Y軸巡回の最小角度
    public float patrolMaxAngleY = 45f;  // Y軸巡回の最大角度
    public float trackingSpeed = 5f;    // プレイヤーを追尾する時の速度

    private bool isTrackingPlayer = false; // プレイヤーを追尾しているか
    private float currentPatrolAngleY;    // Y軸の現在の巡回角度
    private bool isPatrolRotatingRightY = true;  // Y軸巡回方向

    void Start()
    {
        currentPatrolAngleY = transform.localEulerAngles.y;
   
    }

    void Update()
    {
        // プレイヤーが範囲内にいるかどうかを検知
        isTrackingPlayer = DetectPlayer();

        if (isTrackingPlayer)
        {
            // プレイヤーを追尾
            TrackPlayer();
        }
        else
        {
            // 巡回モード
            Patrol();
        }
    }

    // プレイヤーが検知範囲内かつ遮蔽されていないかを確認
    bool DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // プレイヤーが検知範囲内にいるか確認
        if (distanceToPlayer <= detectionRange && angleToPlayer <= detectionAngle / 2f)
        {
            // 判定範囲内にいるか確認
            if (distanceToPlayer <= validRange)
            {
                // プレイヤーとSpotlightの間に障害物がないかRaycastで確認
                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionMask))
                {
                    // プレイヤーは判定範囲内で遮蔽されていない
                    return true;
                }
            }
        }
        return false;
    }

    // プレイヤーを追尾するロジック
    void TrackPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * trackingSpeed);
    }

    // 巡回ロジック、X、Y、Z方向の範囲を調整可能
    void Patrol()
    {
        // Y軸方向の巡回
        if (isPatrolRotatingRightY)
        {
            currentPatrolAngleY += patrolSpeed * Time.deltaTime;
            if (currentPatrolAngleY >= patrolMaxAngleY)
            {
                isPatrolRotatingRightY = false;
            }
        }
        else
        {
            currentPatrolAngleY -= patrolSpeed * Time.deltaTime;
            if (currentPatrolAngleY <= patrolMinAngleY)
            {
                isPatrolRotatingRightY = true;
            }
        }

      
        // Spotlightのローカル回転を設定（X、Y、Z軸で巡回）
        Quaternion patrolRotation = Quaternion.Euler(30, currentPatrolAngleY, 0);
        transform.localRotation = patrolRotation;
    }

    // Gizmosを使用して検知範囲と判定範囲を描画
    void OnDrawGizmos()
    {
        // 検知範囲（大きい範囲）を描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 判定範囲（小さい範囲）を描画
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, validRange);

        // Spotlightの検知角度を描画
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftBoundary * detectionRange);
        Gizmos.DrawRay(transform.position, rightBoundary * detectionRange);
    }
}
