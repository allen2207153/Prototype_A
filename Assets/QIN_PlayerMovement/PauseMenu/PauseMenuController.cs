//using Autodesk.Fbx;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private PauseMenuInput _pInput;
    [SerializeField] private GameObject _PauseMenuUI;
    public static bool GameIsPaused { get; private set; } = false;
    private void Start()
    {
        _pInput = new PauseMenuInput();
        _pInput.Enable();
    }
    void Update()
    {
        if (_pInput.UI.Menu.triggered)
        {
            if (GameIsPaused)
            {
                ResumeTheGame();
            }
            else
            {
                Pause();
            }
        }
    }
    private void Pause()
    {
        _PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void ResumeTheGame()
    {
        _PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void BackToTitle()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneSwitcher.Instance.Loading(1f, "TitleScene");
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
