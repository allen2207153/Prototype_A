using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using UnityEngine.UIElements;

[Serializable]
public class VCamDate 
{
     public CinemachineVirtualCamera _vCam;
}


public class CameraChange : MonoBehaviour
{
    [Header("ブレンドしたいカメラ")]
    [SerializeField]
    VCamDate[] _vCamList;
    [Header("PlayerMovementスクリプト")]
    [SerializeField] private Yako_PlayerMovement _playerMovement;

    [Header("現在のカメラ")]
    public CinemachineVirtualCamera _nowvCam;

    [Header("優先度の値")]
    [SerializeField] private int _hPriority = 100;//高い優先度
    [SerializeField] private int _rPriority = 5;//低い優先度

    // Start is called before the first frame update
    void Start()
    {
        _nowvCam = _vCamList[0]._vCam; ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change(int c)
    {

        // 現在のカメラを更新
        _nowvCam = _vCamList[c]._vCam;
        // 優先度を変更
        _nowvCam.Priority = _hPriority;

        // 他カメラの優先度をリセット
        for (int i = 0; i < _vCamList.Length; i++)
        {
            if (i != c)
            {
                _vCamList[i]._vCam.Priority = _rPriority;
            }
        }

        // PlayerMovementスクリプトに更新したカメラを設定
        _playerMovement._vCam = _nowvCam;
    }
    
}
