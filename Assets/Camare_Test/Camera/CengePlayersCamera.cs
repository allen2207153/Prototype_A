using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CengePlayersCamera : MonoBehaviour
{
    [SerializeField] private GameObject _zone0;
    [SerializeField] private GameObject _zone1;
    [SerializeField] private CameraChenge cameraChenge; // CameraChenge コンポーネントを持つオブジェクト

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == _zone0) { cameraChenge.Cange(1); }
        if(other.gameObject == _zone1) { cameraChenge.Cange(2); }

    }
}
