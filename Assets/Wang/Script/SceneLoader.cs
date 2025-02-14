using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public PlayableDirector playableDirector; // TimelineのPlayableDirector
    public string nextSceneName; // 切り替えたいシーンの名前
    private bool hasTriggered = false; // すでにトリガーされたか
    private bool timelineFinished = false; // Timeline が終わったか

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += OnTimelineFinished; // Timeline終了時に関数を呼び出す
        }
    }

    private void Update()
    {
        // もし Timeline が終了した後 or プレイヤーがボタンを押したら、シーンを切り替える
        if (hasTriggered && (timelineFinished || Input.anyKeyDown))
        {
            LoadNextScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // すでにトリガーされた場合は何もしない
        if (hasTriggered || !other.CompareTag("imouto")) return;

        hasTriggered = true; // トリガー済みに設定
        playableDirector.Play(); // Timelineを再生
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        timelineFinished = true; // Timeline が終了
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName); // シーンをロード
    }
}
