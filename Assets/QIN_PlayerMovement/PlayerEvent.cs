using System;
using UnityEngine;

public class PlayerEvent
{
    public static event Action<bool> CheckHanging;
    public static void CallCheckHanging(bool checkHanging)
    {
        CheckHanging?.Invoke(checkHanging);
    }

    public static event Action<Collider, bool> CheckCollider;
    public static void CallCheckCollider(Collider collider, bool checkEnterOrExit)
    {
        CheckCollider?.Invoke(collider, checkEnterOrExit);
    }
}
