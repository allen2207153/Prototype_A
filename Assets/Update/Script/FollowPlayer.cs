using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // プレイヤー Transform
    public float speed = 5f; // 追蹤速度
    public float rotationSpeed = 720f; // 旋轉速度

    private CharacterController characterController;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // 忽略Y軸，確保角色只在X和Z軸上移動

        if (direction.magnitude > 0.1f)
        {
            Vector3 moveDirection = direction.normalized;
            characterController.Move(moveDirection * speed * Time.deltaTime);

            // 旋轉角色面向玩家
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // 設置動畫參數
            animator.SetFloat("Speed", characterController.velocity.magnitude);
        }
        else
        {
            // 當角色停止時，將動畫速度設置為0
            animator.SetFloat("Speed", 0);
        }
    }
}
