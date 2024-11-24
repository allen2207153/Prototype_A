using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定されたオブジェクトに対して落下アニメーションを伴う力を加えるコントローラー
public class ObjectFallController : MonoBehaviour
{
    // パラメータ設定
    public GameObject targetObject;// 落下させる対象のオブジェクト
    public Animator playerAnimator; // プレイヤーのアニメーターコンポーネント
    public float animationTime = 0.4f;//アニメーション遷移時間
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

        
        yield return new WaitForSeconds(animationTime);
        DisableTrigger();
        // オブジェクトに力を加える処理
        rb.isKinematic = false; 
        playerAnimator.SetBool("isPush", false);
        Invoke("activeKinematic", 1.3f);
    }

    // オブジェクトの動きを停止するメソッド
    void activeKinematic()
    {
        
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void DisableTrigger()
    {
        gameObject.SetActive(false);
    }
}