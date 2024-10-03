using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToTitleScene : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "TitleScene"; // 切り替えるシーンの名前
    [SerializeField] private string playerTag = "Player"; // プレイヤーのタグ（ここでは "Player" に設定）
    public float fadeDuration;
    // プレイヤーがトリガーに触れたときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤータグを持つオブジェクトがトリガーに触れたかを確認
        if (other.CompareTag(playerTag))
        {
            StartCoroutine(FadeAndActivate());
        }
    }

    private IEnumerator FadeAndActivate()
    {
        // フェードイン
        FadeCanvas.Instance.FadeIn();
        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(titleSceneName);

        // フェードアウト
        FadeCanvas.Instance.FadeOut();
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(fadeDuration);
    }
}
