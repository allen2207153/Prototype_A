using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    public GameObject targetObject;       // 指定的物件
    public Vector3 fallDirection = Vector3.forward; // 倒下的方向
    public float fallForce = 10f;         // 倒下的力量大小

    private Rigidbody rb;

    void Start()
    {
        // 检查并获取目标物件的 Rigidbody 组件
        if (targetObject != null)
        {
            rb = targetObject.GetComponent<Rigidbody>();

            // 确保目标物件初始时不受物理影响
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    public void TriggerFall()
    {
        // 当触发倒下事件时，取消 kinematic，让物件受到物理作用
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(fallDirection.normalized * fallForce, ForceMode.Impulse);

            // 倒下后在一定时间后静止
            Invoke("StopMovement", 1f); // 1秒后停止
        }
    }

    void StopMovement()
    {
        // 停止物件的移动和旋转
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // 再次设置为静止
    }
}
