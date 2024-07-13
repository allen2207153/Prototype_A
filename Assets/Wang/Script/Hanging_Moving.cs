using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Hanging_Moving : MonoBehaviour
{
    public Transform man;
    public Transform girl;
    public float followSpeed = 2f;
    public bool ikOn;
    public Vector3 initialOffset;
     void Awake()
    {
        
    }

     void Update()
    {
        ikOn = GetComponent<Test_IKSystem>().ikActive;
        Debug.Log(ikOn);
        if(ikOn)
        {
            girl.position = Vector3.Lerp(girl.position, man.position + initialOffset, followSpeed * Time.deltaTime);
            Debug.Log("isHanging");
        }
        else
        {
            Debug.Log("is not hanging");
        }
    }
}
