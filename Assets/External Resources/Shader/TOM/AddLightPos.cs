using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddlightPos : MonoBehaviour
{
    public Renderer[] renderers;
    public Light[] lights = new Light[16];
    private Vector4[] lightPositions;
    public float[] lightType;
    private MaterialPropertyBlock propertyBlock;

    // Start is called before the first frame update
    void Start()
    {
        lightPositions = new Vector4[lights.Length];
        lightType = new float[lights.Length];
        propertyBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < lights.Length; ++i)
        {
            Vector3 lightPos = lights[i].transform.position;
            lightPositions[i] = new Vector4(lightPos.x, lightPos.y, lightPos.z, 1.0f);
            if (lights[i].type == LightType.Point)
            {
                lightType[i] = 1.0f;
            }
            else if (lights[i].type == LightType.Spot)
            {
                lightType[i] = 2.0f;
            }
        }

        foreach(Renderer rend in renderers)
        {
            rend.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVectorArray("_AdditionalLightPos", lightPositions);
            propertyBlock.SetFloatArray("_LightType", lightType);
            rend.SetPropertyBlock(propertyBlock);
        }
    }
}
