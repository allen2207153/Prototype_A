using System;
using System.Collections.Generic;

/// <summary>
/// 新たなイベントを作成する場合はここに追加してください
/// </summary>
public enum GameEvents
{
    SampleEvent,//使用例
    PlayerDied,//プレイヤが死亡するイベント
    PlayerRespawn,//プレイヤがRespawnするイベント
    PlayerUpdateRespawnPoint,

}


public class EventSystem : Singleton<EventSystem>
{
    private Dictionary<GameEvents, Delegate> eventDictionary = new Dictionary<GameEvents, Delegate>();

    /// <summary>
    /// 注意：OnEnable()に入れること。リスナー（関数）を指定されたイベントに追加します、イベントが存在しない場合、新しく登録します。
    /// </summary>
    /// <param name="eventName">イベント</param>
    /// <param name="listener">追加する関数</param>
    public void StartListening(GameEvents eventName, Action listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = (Action)thisEvent + listener;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 注意：OnEnable()に入れること。リスナー（関数）を指定されたイベントに追加します、イベントが存在しない場合、新しく登録します。
    /// </summary>
    /// <typeparam name="T">注意：１、できる限り使わないこと。２、命名を被らないこと</typeparam>
    /// <param name="eventName">イベント（命名を被らないことを確認ください）</param>
    /// <param name="listener">追加する関数（できる限り使わないください）</param>
    public void StartListening<T>(GameEvents eventName, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = (Action<T>)thisEvent + listener;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }
    /// <summary>
    /// 注意：OnDisable()に入れること。リスナーを指定されたイベントから削除します
    /// </summary>
    /// <param name="eventName">イベント</param>
    /// <param name="listener">追加された関数</param>
    public void StopListening(GameEvents eventName, Action listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            eventDictionary[eventName] = (Action)thisEvent - listener;
        }
    }

    /// <summary>
    /// イベントを呼び出します
    /// </summary>
    /// <param name="eventName">イベント</param>
    public void TriggerEvent(GameEvents eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out Delegate thisEvent))
        {
            ((Action)thisEvent)?.Invoke();
        }
    }
}
