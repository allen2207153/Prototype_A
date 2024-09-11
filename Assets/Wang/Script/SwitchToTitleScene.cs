using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToTitleScene : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "TitleScene"; // 切り替えるシーンの名前
    [SerializeField] private string playerTag = "Player"; // プレイヤーのタグ（ここでは "Player" に設定）

    // プレイヤーがトリガーに触れたときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤータグを持つオブジェクトがトリガーに触れたかを確認
        if (other.CompareTag(playerTag))
        {
            // タイトルシーンに切り替え
            SceneManager.LoadScene(titleSceneName);
        }
    }
}
