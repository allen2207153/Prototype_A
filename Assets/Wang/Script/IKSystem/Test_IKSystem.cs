using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_IKSystem : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform lookObj = null;

    [SerializeField] private bool _grabHand;
    [SerializeField] private bool isHoldingHand;
    private bool wasHoldingHand = false; // 前のフレームの状態を保持

    private float ikWeightRightHand = 0f;
    private float ikWeightLeftHand = 0f;
    private float lookAtWeight = 0f;

    public float transitionSpeed = 0.05f;  // IK重みの遷移速度
    public Vector3 handPositionOffset = new Vector3(0, 0, 0);  // 手の位置のオフセット
    public Vector3 handRotationOffset = new Vector3(0, 0, 0);  // 手の回転のオフセット

    void Start()
    {
        animator = GetComponent<Animator>();

        _grabHand = GetComponent<PlayerMovement>()._grabHandFlag;
        isHoldingHand = GameObject.Find("imouto").GetComponent<FollowPlayer>().isHoldingHands;
        wasHoldingHand = isHoldingHand;  // 初期状態を保存
    }

    void Update()
    {
        _grabHand = GetComponent<PlayerMovement>()._grabHandFlag;
        isHoldingHand = GameObject.Find("imouto").GetComponent<FollowPlayer>().isHoldingHands;

        // isHoldingHand が true になった瞬間に振動
        if (isHoldingHand && !wasHoldingHand)
        {
            StartCoroutine(VibrateController(0.1f, 0.5f));  // 0.1秒間、振動強度0.5
        }
        wasHoldingHand = isHoldingHand;  // 前回の状態を更新
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (isHoldingHand)
            {
                ikActive = true;
                ikWeightRightHand = Mathf.Lerp(ikWeightRightHand, 1f, transitionSpeed);
                ikWeightLeftHand = Mathf.Lerp(ikWeightLeftHand, 1f, transitionSpeed);
                lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, transitionSpeed);
            }
            else
            {
                ikActive = false;
                ikWeightRightHand = Mathf.Lerp(ikWeightRightHand, 0f, transitionSpeed);
                ikWeightLeftHand = Mathf.Lerp(ikWeightLeftHand, 0f, transitionSpeed);
                lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, transitionSpeed);
            }

            if (lookObj != null)
            {
                animator.SetLookAtWeight(lookAtWeight);
                animator.SetLookAtPosition(lookObj.position);
            }

            if (rightHandObj != null)
            {
                Vector3 targetPosition = rightHandObj.position + handPositionOffset;
                Quaternion targetRotation = rightHandObj.rotation * Quaternion.Euler(handRotationOffset);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeightRightHand);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeightRightHand);
                animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
                animator.SetIKRotation(AvatarIKGoal.RightHand, targetRotation);
            }

            if (leftHandObj != null)
            {
                Vector3 targetPosition = leftHandObj.position + handPositionOffset;
                Quaternion targetRotation = leftHandObj.rotation * Quaternion.Euler(handRotationOffset);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeightLeftHand);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeightLeftHand);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPosition);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, targetRotation);
            }
        }
    }

    IEnumerator VibrateController(float duration, float intensity)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(intensity, intensity);
            yield return new WaitForSeconds(duration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
