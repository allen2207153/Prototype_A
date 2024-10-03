using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class Test_IKSystem : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform lookObj = null;

    [SerializeField] private bool _grabHand;
    [SerializeField] private bool isHoldingHand;

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
    }

    void Update()
    {
        _grabHand = GetComponent<PlayerMovement>()._grabHandFlag;
        isHoldingHand = GameObject.Find("imouto").GetComponent<FollowPlayer>().isHoldingHands;
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (_grabHand || isHoldingHand)
            {
                ikActive = true;
                // 重みを時間と共に1に近づける
                ikWeightRightHand = Mathf.Lerp(ikWeightRightHand, 1f, transitionSpeed);
                ikWeightLeftHand = Mathf.Lerp(ikWeightLeftHand, 1f, transitionSpeed);
                lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, transitionSpeed);
            }
            else
            {
                ikActive = false;
                // 重みを時間と共に0に減少させる
                ikWeightRightHand = Mathf.Lerp(ikWeightRightHand, 0f, transitionSpeed);
                ikWeightLeftHand = Mathf.Lerp(ikWeightLeftHand, 0f, transitionSpeed);
                lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, transitionSpeed);
            }

            // 設置目標位置と重み
            if (lookObj != null)
            {
                animator.SetLookAtWeight(lookAtWeight);
                animator.SetLookAtPosition(lookObj.position);
            }

            // 設置右手目標位置と重み
            if (rightHandObj != null)
            {
                Vector3 targetPosition = rightHandObj.position + handPositionOffset;
                Quaternion targetRotation = rightHandObj.rotation * Quaternion.Euler(handRotationOffset);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeightRightHand);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeightRightHand);
                animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
                animator.SetIKRotation(AvatarIKGoal.RightHand, targetRotation);
            }

            // 設置左手目標位置と重み
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
}