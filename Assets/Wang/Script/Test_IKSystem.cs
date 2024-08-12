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
    public bool followPlayer;
    public bool _grabHand;

    void Start()
    {
        animator = GetComponent<Animator>();
        followPlayer = GetComponent<FollowPlayer>();
        _grabHand = GetComponent<PlayerMovement>()._grabHandFlag;
    }

    void Update()
    {
        
    }
    void OnAnimatorIK()
    {
        if (animator)
        {
            // 如果 IK 被激活
            if (ikActive)
            {
                _grabHand = true;
                // 設置目標位置和權重
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // 設置右手目標位置和權重
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                }

                // 設置左手目標位置和權重
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                }
            }
            // 如果 IK 被禁用，重置權重
            else
            {
                _grabHand = false;
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }


}