using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEvent : MonoBehaviour
{
    //EventSystemを使用する例------------------------------------------

    //イベントの登録
    private void OnEnable()
    {
        EventSystem.Instance.StartListening(GameEvents.SampleEvent, SayHello);
    }

    //関数内容
    public void SayHello()
    {
        Debug.Log("Hello!!");
    }

    public void Start()
    {
        //イベントの使用
        EventSystem.Instance.TriggerEvent(GameEvents.SampleEvent);
    }

    //イベントの登録を解除
    private void OnDisable()
    {
        EventSystem.Instance.StopListening(GameEvents.SampleEvent, SayHello);
    }

}
