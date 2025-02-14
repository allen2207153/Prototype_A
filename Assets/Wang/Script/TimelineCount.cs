using UnityEngine;
using UnityEngine.Playables;

public class TimelineCount : MonoBehaviour
{
    [SerializeField] private TimelineTrigger trigger1;
    [SerializeField] private TimelineTrigger trigger2;
    [SerializeField] private TimelineTrigger trigger3;
    [SerializeField] private PlayableDirector finalTimeline; // 3つのトリガーが完了した後に再生するTimeline

    private void Start()
    {
        // TimelineTrigger にこの TimelineCount を設定
        trigger1.timelineCount = this;
        trigger2.timelineCount = this;
        trigger3.timelineCount = this;
    }

    public void CheckAllTriggers()
    {
        // すべてのトリガーが作動済みなら、Timeline を再生
        if (trigger1.HasTriggered() && trigger2.HasTriggered() && trigger3.HasTriggered())
        {
            Debug.Log("All triggers activated! Playing final timeline...");
            finalTimeline.Play();
        }
    }
}
