using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Hanging_Moving : MonoBehaviour
{
    public Transform man;
    public Transform girl;
    public float followSpeed = 2f;
    private bool ikOn;
    public Vector3 initialOffset;

     void Awake()
    {
        ikOn = GetComponent<Test_IKSystem>().ikActive;
    }

     void Update()
    {
        if(ikOn)
        {
            girl.position = Vector3.Lerp(girl.position, man.position + initialOffset, followSpeed * Time.deltaTime);
            Debug.Log("isHanging");
        }
    }
}
