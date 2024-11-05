using System;
using UnityEngine;

public class PlayerEvent : Singleton<PlayerEvent>
{
    //このスクリプトへの追加・削除禁止
    public event Action<bool> CheckHanging;
    public void CallCheckHanging(bool checkHanging)
    {
        CheckHanging?.Invoke(checkHanging);
    }

    public event Action<Collider, bool> CheckCollider;
    public void CallCheckCollider(Collider collider, bool checkEnterOrExit)
    {
        CheckCollider?.Invoke(collider, checkEnterOrExit);
    }
    //ここに追加しないでください
    //EventSytemにしてください
}
