using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEvent")]
public class FadeEvent : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaiesd;

    /// <summary>
    /// 黒になる
    /// </summary>
    /// <param name="duration">経過時間</param>
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }
    /// <summary>
    /// 透明になる
    /// </summary>
    /// <param name="duration">経過時間</param>
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    private void RaiseEvent(Color target, float duration, bool fadeIn)
    {
        OnEventRaiesd?.Invoke(target, duration, fadeIn);
    }
}
