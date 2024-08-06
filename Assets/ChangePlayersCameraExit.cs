using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayersCameraExit : MonoBehaviour
{
    [SerializeField] private Zone[] zones;

    [SerializeField] private CameraChange cameraChange; // CameraChange コンポーネントを持つオブジェクト

    

    private void OnTriggerExit(Collider other)
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
}

