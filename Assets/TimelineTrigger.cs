using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector; // 再生するタイムライン
    private bool _hasTriggered = false; // 既にトリガーされたかどうかを判定するフラグ
    public TimelineCount timelineCount; // TimelineCountを参照

    private void OnTriggerEnter(Collider other)
    {
        // 妹（imouto）が範囲内に入った場合、かつまだトリガーされていない場合
        if (other.CompareTag("imouto") && !_hasTriggered)
        {
            _hasTriggered = true; // トリガー済みとしてフラグを設定
            playableDirector.Play(); // タイムラインを再生
            timelineCount.CheckAllTriggers(); // TimelineCountに通知
            DisableTrigger(); // トリガーを無効化
        }
    }

    public bool HasTriggered()
    {
        return _hasTriggered; // _hasTriggeredの状態を取得
    }

    private void DisableTrigger()
    {
        GetComponent<Collider>().enabled = false; // 現在のトリガーを無効化
    }
}
