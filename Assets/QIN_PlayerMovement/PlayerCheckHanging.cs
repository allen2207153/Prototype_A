using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckHanging : MonoBehaviour
{
    [Header("観測用")]
    [SerializeField] private bool _isCheckHangingOn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HangingCollider")
        {
            _isCheckHangingOn = true;
            PlayerEvent.CallCheckHanging(_isCheckHangingOn);
        }
        else
        {
            _isCheckHangingOn = false;
            PlayerEvent.CallCheckHanging(_isCheckHangingOn);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HangingCollider")
        { 
            _isCheckHangingOn = false;
            PlayerEvent.CallCheckHanging(_isCheckHangingOn);
        }
    }
}

