using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;
    private void Awake()
    {
        //インスタンスの設定
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public FadeEvent _fadeEvent;

    //[SerializeField] private float _fadeDuration;
    IEnumerator LoadSceneCoroutine(float _fadeDuration, string sceneName)
    {
        while (true)
        {
            _fadeEvent.FadeIn(_fadeDuration);
            yield return new WaitForSeconds(_fadeDuration + 0.4f);
            Debug.Log("SceneLoadCompitteee!");//TODO: シーンを切り替え
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
            //yield return new WaitForSeconds(_fadeDuration);
            _fadeEvent.FadeOut(_fadeDuration + 0.2f);
            yield break;
        }
    }
    /// <summary>
    /// シーンをFadeInと共に切り替え
    /// </summary>
    /// <param name="_fadeDuration">FadeIn変化スピード</param>
    /// <param name="sceneName">シーンの名前</param>
    public void Loading(float _fadeDuration,string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(_fadeDuration, sceneName));
    }
}
