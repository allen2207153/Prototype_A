using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    public static FadeCanvas Instance;
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

    [SerializeField] private Image _fadeImage;

    [Header("イベントリスナー")]
    [SerializeField] private FadeEvent _fadeEvent;

    private void OnEnable()
    {
        _fadeEvent.OnEventRaiesd += OnFadeEvent;
    }
    private void OnDisable()
    {
        _fadeEvent.OnEventRaiesd -= OnFadeEvent;
    }
    private void OnFadeEvent(Color target, float duration,bool fadeIn)
    {
        _fadeImage.DOBlendableColor(target, duration);
    }

}
