using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSceneHandler : MonoBehaviour
{
    public CharacterController characterController;  // キャラクターのCharacterController
    public Animator animator;                        // キャラクターのAnimator

    private void Start()
    {
        // シーンの読み込み完了時に呼ばれるイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // オブジェクトが破壊される際にイベントを解除して、メモリリークを防止
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーン読み込み完了時に呼ばれるコールバック
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // CharacterController と Animator を有効にする
        EnableCharacter();
    }

    // キャラクターのコントローラーとアニメーターを有効化
    private void EnableCharacter()
    {
        if (characterController != null)
        {
            characterController.enabled = true;
            Debug.Log("CharacterController が有効化されました");
        }

        if (animator != null)
        {
            animator.enabled = true;
            Debug.Log("Animator が有効化されました");
        }
    }
}