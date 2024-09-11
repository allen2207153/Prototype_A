using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform rigPoint;
    public Animator animator;
    public float followDistance = 2.0f; // NPC 跟隨的距離
    public float stopDistance = 1.5f;   // NPC 停止跟隨的距離
    [Range(0.0f, 3.0f)]
    public float activationRadius;      // 牽手的觸發範圍
    public float speedLerpRate = 5.0f;  // 速度插值的平滑度
    public float runSpeedMultiplier = 1.5f;  // 跑步速度倍數
    private Animator playerAnimator;  // プレイヤーのアニメーターを取得


    private CharacterController characterController;
    private Vector3 lastPlayerPosition;   // 玩家上一次的位置
    private float playerSpeed;            // 玩家當前的速度
    private float currentNPCSpeed;        // NPC 當前的速度

    public KeyCode holdHandKey = KeyCode.H;  // 牽手按鍵
    public bool isHoldingHands = false;     // 是否牽手
    public bool canHold = false;            // 是否可以牽手
    public Vector3 initialOffset;           // 起始偏移量
    [SerializeField] private bool playerIK;  // 是否啟用 IK 系統
    [SerializeField] private bool Iksystem;  // 用於存儲 IK 狀態

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
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        playerIK = GameObject.Find("Oniisan").GetComponent<PlayerMovement>()._grabHandFlag;
        currentNPCSpeed =   playerSpeed = playerAnimator.GetFloat("Speed");// プレイヤーのアニメーションパラメータ Speed を取得
        // 計算玩家的速度
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;
        lastPlayerPosition = player.position; // 更新玩家位置
        Debug.Log("current" + currentNPCSpeed);
        // 檢查 NPC 是否可以牽手
        if (distanceToPlayer <= activationRadius)
        {
            canHold = true;
            if (playerIK != false)
            {
                isHoldingHands = true;

                if (isHoldingHands)
                {
                    // 牽手時，NPC 停止移動，並保持朝向玩家
                    
                    FollowAndMoveNPC();
                    transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                    animator.SetFloat("Speed", currentNPCSpeed);
                }
             
            }

        }
        else
        {
            // 退出牽手狀態
            isHoldingHands = false;
            canHold = false;
        }
    }

    // NPC 跟隨玩家邏輯
    private void FollowAndMoveNPC()
    {
        Vector3 followPosition = player.position - player.forward * followDistance;
        Vector3 moveDirection = (followPosition - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // 距離に応じて移動するか停止するかを判断
        if (distanceToPlayer > followDistance)
        {
            if (playerSpeed > 2f ||playerIK==true)  // プレイヤーの Speed パラメータに基づいてランニングかウォーキングを判断
            {
                currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, playerSpeed * runSpeedMultiplier, Time.deltaTime * speedLerpRate);
            }
            else
            {
                currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, playerSpeed, Time.deltaTime * speedLerpRate);
            }

            characterController.Move(moveDirection * currentNPCSpeed * Time.deltaTime);

            // NPCの方向をrigPointに向かって調整
            transform.LookAt(new Vector3(rigPoint.position.x, transform.position.y, rigPoint.position.z));
        }
        else if (distanceToPlayer < stopDistance)
        {
            currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, 0.0f, Time.deltaTime * speedLerpRate);
            animator.SetFloat("Speed", 0.0f);
            lastPlayerPosition = player.position + initialOffset;
        }

        // NPCのアニメーションを更新
        animator.SetFloat("Speed", currentNPCSpeed);
    }

    void OnDrawGizmosSelected()
    {
        // 畫出牽手範圍
        if(playerIK !=false)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(rigPoint.position, activationRadius);
        }
        //else
        //    isHoldingHands = false;
        //canHold = false;
    }
}