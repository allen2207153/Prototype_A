using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CengePlayersCamera : MonoBehaviour
{
    [SerializeField] private GameObject _zone0;
    [SerializeField] private GameObject _zone1;
    [SerializeField] private CameraChange cameraChange; // CameraChange コンポーネントを持つオブジェクト

    private void OnTriggerExit(Collider other)
    {
        //if(other.gameObject == _zoneX){cameraChenge.Cange(X); }
        if(other.gameObject == _zone0) { cameraChange.Change(0); }
        if(other.gameObject == _zone1) { cameraChange.Change(1); }

    }
}
