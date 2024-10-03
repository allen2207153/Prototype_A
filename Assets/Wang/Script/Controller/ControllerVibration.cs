using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibration : MonoBehaviour
{
    // 振動を開始する
    public void StartVibration(float lowFrequency, float highFrequency, float duration)
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency); // モーターの振動速度を設定
            StartCoroutine(StopVibrationAfterDelay(duration));  // 一定時間後に振動を止める
        }
        else
        {
            Debug.Log("Gamepad is not connected.");
        }
    }

    // 一定時間後に振動を止める
    private IEnumerator StopVibrationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f); // 振動を止める
        }
    }
}
