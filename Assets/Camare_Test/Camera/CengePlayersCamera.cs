using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Zone
{
    public GameObject zoneObject;
    public int cameraIndex;
}
public class CengePlayersCamera : MonoBehaviour
{
    [SerializeField] private Zone[] zones;
    
    [SerializeField] private CameraChange cameraChange; // CameraChange コンポーネントを持つオブジェクト

    private void OnTriggerEnter(Collider other)
    {
        // 各ゾーンをチェック
        foreach (Zone zone in zones)
        {
            if (other.gameObject == zone.zoneObject)
            {
                cameraChange.Change(zone.cameraIndex);
                break; 
            }
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        // 各ゾーンをチェック
        foreach (Zone zone in zones)
        {
            if (other.gameObject == zone.zoneObject)
            {
                cameraChange.Change(zone.cameraIndex);
                break;
            }
        }
    }*/
}
