using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapReset : MonoBehaviour
{
    [SerializeField] private Rigidbody woodRigidbody; // 木頭的 Rigidbody
    [SerializeField] private BoxCollider woodCollider; // 木頭作為牆壁的 BoxCollider
    [SerializeField] private Transform woodStartPos; // 木頭的初始位置
    [SerializeField] private Transform playerStartPos; // 玩家重生位置
    [SerializeField] private Transform imoutoStartPos; // Imouto 重生位置
    [SerializeField] private GameObject player; // 玩家物件
    [SerializeField] private GameObject imouto; // Imouto 物件

    public float fadeDuration = 1.0f;    // フェードイン・フェードアウトの時間

    private Vector3 originalWoodPos; // 儲存木頭的初始位置
    private Quaternion originalWoodRot; // 儲存木頭的初始旋轉

    private void Start()
    {
        // 存儲木頭的初始位置和旋轉
        originalWoodPos = woodStartPos.position;
        originalWoodRot = woodStartPos.rotation;

        // 確保木頭一開始是靜態的
        woodRigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 如果木頭碰到 Player 或 Imouto，就重置場景
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("imouto"))
        {
            ResetScene();
        }

        if (other.CompareTag("Obstacle"))
        {
            woodRigidbody.isKinematic = true;
            woodCollider.isTrigger = false;
            woodCollider.enabled = true;
        }
    }
 

    private void ResetScene()
    {

        StartCoroutine(Reset());
       
    }
    private IEnumerator Reset()
    {
        // フェードインを開始
        FadeCanvas.Instance.FadeIn();
       
        yield return new WaitForSeconds(fadeDuration); // フェードインの完了を待つ

        // 重置玩家與 Imouto 位置
        player.transform.position = playerStartPos.position;
        player.transform.rotation = playerStartPos.rotation;

        imouto.transform.position = imoutoStartPos.position;
        imouto.transform.rotation = imoutoStartPos.rotation;

        // 重置木頭回到原位
        woodRigidbody.isKinematic = true; // 讓木頭靜止
        woodCollider.enabled = false;
        woodCollider.isTrigger = true;


        // 恢復木頭位置和旋轉
        woodRigidbody.position = originalWoodPos;
        woodRigidbody.rotation = originalWoodRot;

        // 確保 Rigidbody 的速度與加速度歸零，避免異常物理效果
        woodRigidbody.velocity = Vector3.zero;
        woodRigidbody.angularVelocity = Vector3.zero;

        // フェードアウトを開始
        FadeCanvas.Instance.FadeOut();
        yield return new WaitForSeconds(fadeDuration); // フェードアウトの完了を待つ
    }
}
