using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;                // プレイヤーの位置
    public Transform rigPoint;              // リグの位置
    public Animator animator;               // NPCのアニメーター
    public Animator playerAnimator;
    public float followDistance = 2.0f;     // NPCがプレイヤーを追従する距離
    public float stopDistance = 1.5f;       // NPCが追従を停止する距離
    [Range(0.0f, 3.0f)] public float activationRadius; // 牽手を開始する範囲
    public float speedLerpRate = 5.0f;      // 移動速度の補間率
    public float runSpeedMultiplier = 1.5f; // 走るときの速度倍数
    private CharacterController characterController;
    private Vector3 lastPlayerPosition;     // プレイヤーの前回の位置
    private float playerSpeed;              // プレイヤーの現在の速度
    private float currentNPCSpeed;          // NPCの現在の速度

    public bool isHoldingHands = false;     // 手をつないでいるか
    public bool canHold = false;            // 手をつなげるか
    public Vector3 initialOffset;           // 初期位置のオフセット
    [SerializeField] private bool playerIK; // IKシステムの有効化フラグ

    private ControllerVibration controllerVibration;

    //追加時間：20240914＿八子遥輝
    private PlayerMovement playerMovement;
    private static readonly int isStandingHash = Animator.StringToHash("isStanding");
    private static readonly int isWalkingHash = Animator.StringToHash("isWalking");

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        characterController = GetComponent<CharacterController>();
        lastPlayerPosition = player.position + initialOffset; // 初始位置
      

        currentNPCSpeed = 0f; // 初始速度設為 0

        playerIK = GameObject.Find("Oniisan").GetComponent<PlayerMovement>()._grabHandFlag;

        //追加時間：20240914＿八子遥輝
        playerMovement = GameObject.Find("Oniisan").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerAnimator = GameObject.Find("Oniisan").GetComponent<Animator>();
        controllerVibration = GetComponent<ControllerVibration>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position); // プレイヤーとの距離を計算
        playerIK = GameObject.Find("Oniisan").GetComponent<PlayerMovement>()._grabHandFlag; // プレイヤーフラグ更新


        lastPlayerPosition = player.position;

        // 手をつなぐ条件チェック
        canHold = (distanceToPlayer <= activationRadius);
        if (canHold && playerIK)
        {
            isHoldingHands = true;
            FollowAndMoveNPC(); // NPCを追従させる
            transform.LookAt(new Vector3(player.position.x+initialOffset.x, transform.position.y, player.position.z+initialOffset.z)); // プレイヤーの方向を向く
        }
        else
        {
            isHoldingHands = false;
            canHold = false;
        }

        UpdateAnimationState(); // アニメーションの状態を更新
    }
    

    //追加時間：20240914＿八子遥輝
    private void UpdateAnimationState()
    {
        if (playerMovement != null)
        {
            if (isHoldingHands)
            {
                PlayerMovement.Motion playerMotion = playerMovement.GetMotion();
                bool isStanding = playerMotion == PlayerMovement.Motion.Stand;
                bool isWalking = playerMotion == PlayerMovement.Motion.Walk;

                animator.SetBool(isStandingHash, isStanding);
                animator.SetBool(isWalkingHash, isWalking);
            }
            else
            {
                // 手を繋いでいない場合は立ち状態に
                animator.SetBool(isStandingHash, true);
                animator.SetBool(isWalkingHash, false);
            }
        }
        else
        {
            // プレイヤーが見つからない場合、NPCを立ち状態にする
            animator.SetBool(isStandingHash, true);
            animator.SetBool(isWalkingHash, false);
        }
    }

    // NPC がプレイヤーを追従するロジック
    private void FollowAndMoveNPC()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // **1. 停止追蹤當距離過近**
        if (distanceToPlayer <= stopDistance)
        {
            currentNPCSpeed = 0;
            animator.SetFloat("Speed", 0);
            return;
        }

        // **2. 計算 NPC 目標位置**
        Vector3 targetPosition = player.position - player.forward * followDistance;
        targetPosition.y = transform.position.y; // 讓 NPC 保持在相同的 Y 軸高度

        // **3. 使用 Lerp 平滑移動**
        transform.position = Vector3.Lerp(transform.position, targetPosition, speedLerpRate * Time.deltaTime);

        // **4. 設定移動方向並平滑旋轉**
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedLerpRate * Time.deltaTime);
        }

        // **5. 讓 NPC 速度跟隨玩家**
        playerSpeed = playerAnimator.GetFloat("Speed");
        currentNPCSpeed = playerSpeed;
        animator.SetFloat("Speed", currentNPCSpeed);
    }


    void OnDrawGizmosSelected()
    {
        // 手をつなぐ範囲を表示
        if (playerIK)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(rigPoint.position, activationRadius); // 範囲を描画
        }
    }
}