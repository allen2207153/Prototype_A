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

    
    public static event Action PlayerRespawn;
    /// <summary>
    /// プレイヤーと妹を前記録されたRespawnPointにRespawnする
    /// </summary>
    public static void CallPlayerRespawn()
    {
        PlayerRespawn?.Invoke();
    }
 
    public static event Action UpdateRespawnPoint;
    /// <summary>
    /// プレイヤーと妹の座標を記録し、RespawnPointに更新する
    /// </summary>
    public static void CallUpdateRespawnPoint()
    {
        UpdateRespawnPoint?.Invoke();
    }
}
