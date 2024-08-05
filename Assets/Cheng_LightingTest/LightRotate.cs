using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotate : MonoBehaviour
{
    [SerializeField]
    [Range(0, 180)]public float lightAng;
    
    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = new Vector3(lightAng, 0, 0);
        
    }
}
