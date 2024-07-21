using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float followDistance = 2.0f; // プレイヤーを追従し始める距離
    public float stopDistance = 1.5f; // プレイヤーの追従を停止する距離
    [Range(0.0f, 3.0f)]
    public float activationRadius; // プレイヤーがキーを押してNPCを動かせる距離
    public float speedLerpRate = 5.0f; // 速度を平滑にするためのレート

    private CharacterController characterController;
    [SerializeField]private bool isFollowing = false; // NPCがプレイヤーを追従しているかどうかを示すフラグ
    private Vector3 lastPlayerPosition; // プレイヤーの前回位置
    private float playerSpeed; // プレイヤーの速度
    private float currentNPCSpeed; // 現在のNPCの速度

    public MultiRotationConstraint playerMultiRotationCons;
    public MultiRotationConstraint npcMultiRotationCons;
    public MultiParentConstraint npcHandSync;
    public KeyCode holdHandKey = KeyCode.H;
    [SerializeField]private bool isHoldingHands = false;
    public Vector3 initialOffset; // 初始偏移量，用于保持NPC在玩家身后

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        characterController = GetComponent<CharacterController>();
        lastPlayerPosition = player.position + initialOffset; // 初期プレイヤー位置を設定
        currentNPCSpeed = 0f; // 初期のNPCの速度を0に設定
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // プレイヤーの速度を計算
        playerSpeed = (player.position - lastPlayerPosition).magnitude / Time.deltaTime;
        lastPlayerPosition = player.position; // 前回位置を更新

        // プレイヤーが一定範囲内にいるかどうかをチェック
        if (distanceToPlayer <= activationRadius && Input.GetKeyDown(holdHandKey))
        {
            isHoldingHands = !isHoldingHands;
            isFollowing = !isFollowing; // フラグをトグル
        }

        if (isHoldingHands)
        {
            // 开始牵手
            playerMultiRotationCons.weight = 1.0f;
            npcMultiRotationCons.weight = 1.0f;
            npcHandSync.weight = 1.0f;

            // 检查玩家与NPC的距离
            if (distanceToPlayer < followDistance)
            {
                // 停止NPC的移动，仅调整朝向
                currentNPCSpeed = 0.0f;
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            }
            else
            {
                // 跟随玩家
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0; // 水平面上で移動を保つ

                currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, playerSpeed, Time.deltaTime * speedLerpRate);

                characterController.Move(direction * currentNPCSpeed * Time.deltaTime);

                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            }

            // 设置动画参数
            animator.SetFloat("Speed", currentNPCSpeed);
        }
        else
        {
            // 取消牵手
            playerMultiRotationCons.weight = 0.0f;
            npcMultiRotationCons.weight = 0.0f;
            npcHandSync.weight = 0.0f;

            // NPCがプレイヤーを追従している場合
            if (isFollowing)
            {
                float distance = Vector3.Distance(player.position, transform.position);

                if (distance > followDistance)
                {
                    Vector3 direction = (player.position - transform.position).normalized;
                    direction.y = 0; // 水平面上で移動を保つ

                    currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, playerSpeed, Time.deltaTime * speedLerpRate);

                    characterController.Move(direction * currentNPCSpeed * Time.deltaTime);

                    transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

                    animator.SetFloat("Speed", currentNPCSpeed);
                }
                else if (distance < stopDistance)
                {
                    currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, 0.0f, Time.deltaTime * speedLerpRate);
                    animator.SetFloat("Speed", 0.0f);
                }
            }
            else
            {
                currentNPCSpeed = Mathf.Lerp(currentNPCSpeed, 0.0f, Time.deltaTime * speedLerpRate);
                animator.SetFloat("Speed", 0.0f);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // NPCの周囲にアクティベーション範囲を描画
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}