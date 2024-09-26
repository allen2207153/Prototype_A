using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationTest : MonoBehaviour
{
    private ControllerVibration controllerVibration;

    void Start()
    {
        controllerVibration = GetComponent<ControllerVibration>();
    }

    void Update()
    {
        // 'Space' キーを押すと振動が開始される
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            controllerVibration.StartVibration(0.5f, 0.5f, 1f); // 1秒間、低・高周波数で振動
        }
    }
}