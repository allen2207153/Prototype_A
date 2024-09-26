using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ButtonController : MonoBehaviour
{
    [SerializeField] string _newGameButtonLoadingScene;
    [SerializeField] string _settingButtonLoadingScene;
    [SerializeField] Image _button_PressAnyButton;
    private TitleInput _titleInput;
    private void Start()
    {
        StartBlinking();
        _titleInput=new TitleInput();
        _titleInput.Enable();
    }
    private void Update()
    {
        if (_titleInput.UI.PressAnyButton.triggered)
        {
            LoadingToGameScene();
        }
    }
    private void StartBlinking()
    {
        if (_button_PressAnyButton != null)
        {
            _button_PressAnyButton.DOFade(0.4f, 0.8f).SetLoops(-1, LoopType.Yoyo);
        }
    }

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
            SceneSwitcher.Instance.Loading(1f, _settingButtonLoadingScene);
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
