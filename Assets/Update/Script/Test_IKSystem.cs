using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Test_IKSystem : MonoBehaviour
{
    protected Animator animator;


    public Transform character1;
    public Transform character2;
    public bool ikActive = true;
    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform lookObj = null;
    public float longPressDuration = 1.0f; // 持續時間的閾值
    private float pressTime = 0.0f;
    private bool isPressing = false;
    public float followSpeed = 2.0f;
    public Vector3 initialOffset;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 檢測K鍵按下
        if (Input.GetKeyDown(KeyCode.K))
        {
            isPressing = true;
            pressTime = 0.0f; // 重置計時器
            ikActive = true;
        }

        // 檢測K鍵釋放
        if (Input.GetKeyUp(KeyCode.K))
        {
            isPressing = false;
            ikActive = false;
            initialOffset = character2.position - character1.position;
        }

        // 計算按下時間
        if (isPressing)
        {
            pressTime += Time.deltaTime;

            if (pressTime >= longPressDuration)
            {
                PerformLongPressAction();
               

            }
        }
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            // 如果 IK 被激活
            if (ikActive)
            {
                character2.position = Vector3.Lerp(character2.position, character1.position + initialOffset, followSpeed * Time.deltaTime);
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
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }

                // 設置左手目標位置和權重
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }
            }
            // 如果 IK 被禁用，重置權重
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
    void PerformLongPressAction()
    {
        // 在這裡執行你想要的動作
        Debug.Log("Long press action performed!");
    }
}
