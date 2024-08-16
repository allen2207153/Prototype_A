using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void LoadingToGameScene()
    {
        //TODO:ゲームシーンを記入してください
        //SceneManager.LoadScene("ゲームシーンを記入");
        Debug.Log("NewGame");
    }
    public void LoadingSettingScene()
    {
        //TODO:セッティングシーンを記入してください
        Debug.Log("Setting");
    }
    public void ExitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
