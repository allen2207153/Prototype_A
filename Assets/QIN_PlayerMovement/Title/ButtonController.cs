using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [SerializeField] string _newGameButtonLoadingScene;
    [SerializeField] string _settingButtonLoadingScene;
    public void LoadingToGameScene()
    {
        Debug.Log("NewGame");
        if (_newGameButtonLoadingScene.Length != 0)
        {
            SceneSwitcher.Instance.Loading(1f, _newGameButtonLoadingScene);
        }
    }
    public void LoadingSettingScene()
    {
        Debug.Log("Setting");
        if (_settingButtonLoadingScene.Length != 0)
        {
            SceneSwitcher.Instance.Loading(1f,_settingButtonLoadingScene);
        }
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
