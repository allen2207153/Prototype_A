using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public string nextSceneName; // 切换的场景名称
    public float delay = 3f; // 延迟时间
    private bool isSceneLoading = false; // 防止重复加载
    public float fadeDuration = 1f; // 淡入时间

    private void Start()
    {
        StartCoroutine(DelayedLoad());
    }

    private void Update()
    {
        // 监听控制器的 "Start" 或 "Options" 按钮
        if (Input.GetButtonDown("Submit") && !isSceneLoading)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(delay);
        if (!isSceneLoading) // 确保不会重复执行
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (isSceneLoading) yield break;
        isSceneLoading = true;

        // 调用淡入动画
        if (FadeCanvas.Instance != null)
        {
            FadeCanvas.Instance.FadeIn();

            // 等待淡入动画完成
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                yield return null; // 等待下一帧
            }
        }
        else
        {
            Debug.LogWarning("FadeCanvas.Instance is null, skipping fade.");
        }
        FadeCanvas.Instance.FadeOut();
        // 加载下一个场景
        SceneManager.LoadScene(nextSceneName);
    }
}
