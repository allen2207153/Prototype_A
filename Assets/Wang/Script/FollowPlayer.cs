using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; 
    public Animator animator; 
    public float followDistance = 2.0f; // プレイヤーを追従し始める距離
    public float stopDistance = 1.5f; // プレイヤーの追従を停止する距離
    public KeyCode followKey = KeyCode.F; // 追従を開始するためのキー
    [Range(0.0f, 3.0f)]
    public float activationRadius; // プレイヤーがキーを押してNPCを動かせる距離
    public float speedLerpRate = 5.0f; // 速度を平滑にするためのレート

    private CharacterController characterController;
    private bool isFollowing = false; // NPCがプレイヤーを追従しているかどうかを示すフラグ
    private Vector3 lastPlayerPosition; // プレイヤーの前回位置
    private float playerSpeed; // プレイヤーの速度
    private float currentNPCSpeed; // 現在のNPCの速度

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        characterController = GetComponent<CharacterController>();
        lastPlayerPosition = player.position; // 初期プレイヤー位置を設定
        currentNPCSpeed = 0f; // 初期のNPCの速度を0に設定
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // プレイヤーの速度を計算
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;
        lastPlayerPosition = player.position; // 前回位置を更新

        // プレイヤーが一定範囲内にいるかどうかをチェック
        if (distanceToPlayer <= activationRadius && Input.GetKeyDown(followKey))
        {
            isFollowing = !isFollowing; // フラグをトグル
        }

        // NPCがプレイヤーを追従している場合
        if (isFollowing)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance > followDistance)
            {
                // プレイヤーの方向を計算
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0; // 水平面上で移動を保つ

                // NPCの現在の速度をプレイヤーの速度に平滑に適応
                currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, playerSpeed, Time.deltaTime * speedLerpRate);

                // NPCを移動
                characterController.Move(direction * currentNPCSpeed * Time.deltaTime);

                // NPCをプレイヤーの方向に向ける
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

                // アニメーションのパラメータを設定
                animator.SetFloat("Speed", currentNPCSpeed);
            }
            else if (distance < stopDistance)
            {
                // 移動を停止
                currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, 0.0f, Time.deltaTime * speedLerpRate);
                animator.SetFloat("Speed", 0.0f);
            }
        }
        else
        {
            // NPCがプレイヤーを追従していない場合、速度を0に設定
            currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, 0.0f, Time.deltaTime * speedLerpRate);
            animator.SetFloat("Speed", 0.0f);
        }
    }

    void OnDrawGizmosSelected()
    {
        // NPCの周囲にアクティベーション範囲を描画
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
