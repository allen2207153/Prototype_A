using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定されたオブジェクトに対して落下アニメーションを伴う力を加えるコントローラー
public class ObjectFallController : MonoBehaviour
{
    // パラメータ設定
    public GameObject targetObject;// 落下させる対象のオブジェクト
    public Vector3 fallDirection = Vector3.forward; // 落下の方向
    public float fallForce = 10f;// 落下させる力の強さ
    public Animator playerAnimator; // プレイヤーのアニメーターコンポーネント

    private Rigidbody rb;// 対象オブジェクトの Rigidbody への参照

  
    void Start()
    {
        
        if (targetObject != null)
        {
            rb = targetObject.GetComponent<Rigidbody>();

           
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }
    public void TriggerFall()
    {
        // コルーチンでアニメーションと落下処理を開始
        StartCoroutine(FallWithAnimation());
    }

    // アニメーションとともに力を加えてオブジェクトを落下させるコルーチン
    private IEnumerator FallWithAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isPush", true); 
        }

        
        yield return new WaitForSeconds(0.4f);

        // オブジェクトに力を加える処理
        if (rb != null)
        {
            rb.isKinematic = false; 
            rb.AddForce(fallDirection.normalized * fallForce, ForceMode.Impulse);

            
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isPush", false);
            }
        }

        
        Invoke("StopMovement", 1f);
    }

    // オブジェクトの動きを停止するメソッド
    void StopMovement()
    {
        
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
}