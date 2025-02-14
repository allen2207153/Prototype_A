using UnityEngine;

public class WoodTrap : MonoBehaviour
{
    [SerializeField] private Rigidbody woodRigidbody; // 木頭的 Rigidbody
   [SerializeField] private BoxCollider woodCollider; // 木頭作為牆壁的 BoxCollider

   
    private void Start()
    {
        // 確保木頭一開始是靜態的
        woodRigidbody.isKinematic = true;
        woodCollider.enabled = false; // 一開始不啟動碰撞
    }

    private void OnTriggerEnter(Collider other)
    {
        // 當玩家觸發陷阱時，讓木頭開始掉落
        if (other.CompareTag("imouto"))
        {
            woodRigidbody.isKinematic = false; // 取消 Kinematic，開始掉落
            woodCollider.enabled = true; // 啟動碰撞，變成牆壁
        }
      
    }

    
}
