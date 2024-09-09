using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLightChange : MonoBehaviour
{
    [SerializeField] LightRotate _lightRot;
    [SerializeField] Light _directionLight;
    float sunRot;

    [Header("日光の色")]
    [SerializeField] public Color _sunColor1;
    [SerializeField] public Color _sunColor2;

    [Header("昼ライティング")]
    [SerializeField] public Color _daySky;
    [SerializeField] public Color _dayEquator;
    [SerializeField] public Color _dayGround;

    [Header("黄昏ライティング")]
    [SerializeField] public Color _sunsetSky;
    [SerializeField] public Color _sunsetEquator;
    [SerializeField] public Color _sunsetGround;


    [SerializeField] public float startSunset;
    [SerializeField] public float endSunset;

    private Gradient _dayGradient;
    private Gradient _sunsetGradient;
    void Start()
    {
        //昼のグラテーションを設定
        _dayGradient = CreateGradient(_daySky, _dayEquator, _dayGround);

        //黄昏のグラテーションを設定
        _sunsetGradient = CreateGradient(_sunsetSky, _sunsetEquator, _sunsetGround);
   
        //Direction Lightのコンポーネントを取得
        if(_directionLight == null)
        {
            Debug.LogWarning("Direction Lightが設定されてない");
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        sunRot = _lightRot.lightAng;

        //回転角度が指定範囲内の場合に補間を行う
        if (sunRot < startSunset)
        {
            float t = Mathf.InverseLerp(startSunset, endSunset, sunRot);
            Gradient tempGradient = LerpGradients(_dayGradient, _sunsetGradient, t);

            //環境光の設定(変化の場合)
            RenderSettings.ambientSkyColor = tempGradient.Evaluate(0f); //sky color
            RenderSettings.ambientEquatorColor = tempGradient.Evaluate(0.5f); //equator color
            RenderSettings.ambientGroundColor = tempGradient.Evaluate(1f); //ground color
        
            //Direction Lightの色(変化の場合)
            if(_directionLight != null)
            {
                _directionLight.color = Color.Lerp(_sunColor1, _sunColor2, t);
            }
        }
        else
        {
            //環境光の設定(変化前)
            RenderSettings.ambientSkyColor =  _dayGradient.Evaluate(0f);
            RenderSettings.ambientEquatorColor = _dayGradient.Evaluate(0.5f); //equator color
            RenderSettings.ambientGroundColor = _dayGradient.Evaluate(1f); //ground color

            //Direction Lightの色(変化前)
            if(_directionLight != null)
            {
                _directionLight.color = _sunColor1;
            }
        }
    }

    /// <summary>
    /// 環境光のグラテーション生成
    /// </summary>
    /// <param name="sky"></param>
    /// <param name="equator"></param>
    /// <param name="ground"></param>
    /// <returns></returns>
    Gradient CreateGradient(Color sky, Color equator, Color ground)
    {
        Gradient gradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[3];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[3];

        colorKeys[0] = new GradientColorKey(sky, 0f);   //sky color
        colorKeys[1] = new GradientColorKey(equator, 0.5f); //equator color
        colorKeys[2] = new GradientColorKey(ground, 1f); //ground color

        alphaKeys[0] = new GradientAlphaKey(1f, 0f); //sky color
        alphaKeys[1] = new GradientAlphaKey(1f, 0.5f); //equator color
        alphaKeys[2] = new GradientAlphaKey(1f, 1f); //ground color

        gradient.SetKeys(colorKeys, alphaKeys);

        return gradient;
    }

    /// <summary>
    /// 環境光の補間計算
    /// </summary>
    /// <param name="grad1"></param>
    /// <param name="grad2"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    Gradient LerpGradients(Gradient grad1, Gradient grad2, float t)
    {
        Gradient newGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[grad1.colorKeys.Length];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[grad1.alphaKeys.Length];

        for(int i = 0; i < grad1.colorKeys.Length; ++i)
        {
            colorKeys[i].color = Color.Lerp(grad1.colorKeys[i].color, grad2.colorKeys[i].color ,t);
            colorKeys[i].time = Mathf.Lerp(grad1.colorKeys[i].time, grad2.colorKeys[i].time, t);
        }

        for(int i = 0; i < grad1.alphaKeys.Length; ++i)
        {
            alphaKeys[i].alpha = Mathf.Lerp(grad1.alphaKeys[i].alpha, grad2.alphaKeys[i].alpha, t);
            alphaKeys[i].time = Mathf.Lerp(grad1.alphaKeys[i].time, grad2.alphaKeys[i].time, t);
        }

        newGradient.SetKeys(colorKeys, alphaKeys);

        return newGradient;
    }
}
