using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyChange : MonoBehaviour
{
    [SerializeField] LightRotate _directionLight;
    float sunRot;
    
    [SerializeField] public Material day;
    [SerializeField] public Material sunset;

    [SerializeField] public float startSunset;
    [SerializeField] public float endSunset;


    // Update is called once per frame
    void Update()
    {
        sunRot = _directionLight.lightAng;

        Material tempSkybox;

        if(sunRot < startSunset)
        {
            //回転角度を指定範囲内で正規化
            float t = Mathf.InverseLerp(startSunset, endSunset, sunRot);

            //skyboxの補間
            tempSkybox = LerpMaterials(day, sunset, t);
        }
        else
        {
            tempSkybox = day;
        }

        //skyboxを変更
        RenderSettings.skybox = tempSkybox;
    }

    Material LerpMaterials(Material mat1, Material mat2, float t)
    {
        Material newMat = new Material(mat1);

        //色を補間
        Color mat1Color = mat1.GetColor("_Tint");
        Color mat2Color = mat2.GetColor("_Tint");
        Color tempColor = Color.Lerp(mat1Color, mat2Color, t);
        newMat.SetColor("_Tint", tempColor);

        return newMat;
    }
}
