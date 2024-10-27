using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Obstacle : MonoBehaviour
{
    public ObjectFallController objectFallController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 当玩家接触到触发器
        {
            objectFallController.TriggerFall(); // 触发目标物件倒下
        }
    
}
}
