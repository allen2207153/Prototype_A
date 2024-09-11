using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_SceneTrigger : MonoBehaviour
{
    [SerializeField]private string SceneName;
    [SerializeField] private float durationTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("imouto"))
        {
            SceneSwitcher.Instance.Loading(durationTime, SceneName);
        }
    }
}
