using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToTitleScene : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "TitleScene"; // 切り替えるシーンの名前
    [SerializeField] private string playerTag = "Player"; // プレイヤーのタグ（ここでは "Player" に設定）
    public FadeCanvas FadeCanvas;
    public float fadeDuration;
    // プレイヤーがトリガーに触れたときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤータグを持つオブジェクトがトリガーに触れたかを確認
        if (other.CompareTag(playerTag))
        {
            Sequence fadeSequence = DOTween.Sequence();
            fadeSequence.AppendCallback(() => FadeCanvas.FadeIn())   // 淡入
                        .AppendInterval(fadeDuration)   // 等待淡入完成
                        .AppendCallback(() =>
                        {
                            SceneManager.LoadScene(titleSceneName);
                        })
            .AppendCallback(() => FadeCanvas.FadeOut())  // 执行淡出
                    .AppendInterval(fadeDuration);   // 等待淡出完成

        }
    }
}
