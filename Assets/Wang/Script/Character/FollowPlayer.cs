using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;                // プレイヤーの位置
    public Transform rigPoint;              // リグの位置
    public Animator animator;               // NPCのアニメーター
    public float followDistance = 2.0f;     // NPCがプレイヤーを追従する距離
    public float stopDistance = 1.5f;       // NPCが追従を停止する距離
    [Range(0.0f, 3.0f)] public float activationRadius; // 牽手を開始する範囲
    public float speedLerpRate = 5.0f;      // 移動速度の補間率
    public float runSpeedMultiplier = 1.5f; // 走るときの速度倍数
    private Animator playerAnimator;        // プレイヤーのアニメーター
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
        playerAnimator = player.GetComponent<Animator>();  // プレイヤーのアニメーターを取得

        currentNPCSpeed = 0f; // 初始速度設為 0

        playerIK = GameObject.Find("Oniisan").GetComponent<PlayerMovement>()._grabHandFlag;

        //追加時間：20240914＿八子遥輝
        playerMovement = player.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

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
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); // プレイヤーの方向を向く
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
    private void FollowAndMoveNPC()//更新時間：20240914＿八子遥輝
    {
        Vector3 followPosition = player.position - player.forward * followDistance; // NPCが追従する目標位置
        Vector3 moveDirection = (followPosition - transform.position).normalized;   // 移動方向を計算
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        currentNPCSpeed = 0;
        // プレイヤーの速度をAnimatorから取得
        playerSpeed = playerAnimator.GetFloat("Speed");

        if ( isHoldingHands)
        {
            // プレイヤーの速度を基準にNPCの速度を設定（走っているかどうかで調整）
            currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, playerSpeed * runSpeedMultiplier,  speedLerpRate);
            characterController.Move(moveDirection * currentNPCSpeed * Time.deltaTime); // NPCを移動
        }
        else
        {
            // 停止時は速度を0に設定し、位置を更新
            currentNPCSpeed = 0;
            lastPlayerPosition = player.position + initialOffset;
        }

        // NPCのアニメーションパラメータ「Speed」を更新
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