using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentLightChange : MonoBehaviour
{
    [SerializeField] LightRotate _directionLight;
    float sunRot;

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
    }

    // Update is called once per frame
    void Update()
    {
        sunRot = _directionLight.lightAng;

        //回転角度が指定範囲内の場合に補間を行う
        if (sunRot < startSunset)
        {
            float t = Mathf.InverseLerp(startSunset, endSunset, sunRot);
            Gradient tempGradient = LerpGradients(_dayGradient, _sunsetGradient, t);

            RenderSettings.ambientSkyColor = tempGradient.Evaluate(0f); //sky color
            RenderSettings.ambientEquatorColor = tempGradient.Evaluate(0.5f); //equator color
            RenderSettings.ambientGroundColor = tempGradient.Evaluate(1f); //ground color
        }
        else
        {
            RenderSettings.ambientSkyColor =  _dayGradient.Evaluate(0f);
            RenderSettings.ambientEquatorColor = _dayGradient.Evaluate(0.5f); //equator color
            RenderSettings.ambientGroundColor = _dayGradient.Evaluate(1f); //ground color

        }
    }

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
