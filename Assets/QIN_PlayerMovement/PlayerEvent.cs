using System;


public class PlayerEvent
{
    public static event Action<bool> CheckHanging;
    public static void CallCheckHanging(bool checkHanging)
    {
        CheckHanging?.Invoke(checkHanging);
    }
}
