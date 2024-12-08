using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("imouto"))  
        {
            playableDirector.Play();  //Timelineをプレイする
        }
    }
}
