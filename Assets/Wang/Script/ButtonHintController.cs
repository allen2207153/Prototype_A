using UnityEngine.UI;
using UnityEngine;

public class ButtonHintController : MonoBehaviour
{
    public GameObject buttonPrompt;  // ボタン画像のGameObject (Canvas全体またはImage)

    private bool showPrompt = false;  // bool変数で画像表示の制御

    void Start()
    {
        // 初期状態ではボタン画像を非表示に設定
        buttonPrompt.SetActive(false);
    }

    void Update()
    {
        // bool変数によって表示/非表示を制御
        if (showPrompt)
        {
            buttonPrompt.SetActive(true);  // ボタン画像を表示
        }
        else
        {
            buttonPrompt.SetActive(false); // ボタン画像を非表示
        }
    }

    // プレイヤーがトリガーに入った時に画像を表示
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーがトリガーに入ったかを確認
        {
            showPrompt = true;  // ボタン画像を表示するようにboolをtrueに
        }
    }

    // プレイヤーがトリガーを離れた時に画像を非表示
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            showPrompt = false; // ボタン画像を非表示にする
        }
    }

    // 外部からboolを切り替える場合
    public void SetButtonPrompt(bool value)
    {
        showPrompt = value;
    }
}
