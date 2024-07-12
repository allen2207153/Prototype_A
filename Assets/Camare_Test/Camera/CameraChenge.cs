using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraChenge : MonoBehaviour
{
    [Header("ブレンドしたいカメラ")]
    [SerializeField] private CinemachineVirtualCamera _vCam0;
    [SerializeField] private CinemachineVirtualCamera _vCam1;
    [Header("PlayerMovementスクリプト")]
    [SerializeField] private PlayerMovement_CameraTest _CameraTest;

    [Header("現在のカメラ")]
    public CinemachineVirtualCamera _nowvCam; 
    // Start is called before the first frame update
    void Start()
    {
        _nowvCam = _vCam0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cange(int c)
    {
        
        switch (c)
        {
            case 1:
                //現在のカメラを更新
                _nowvCam = _vCam0;
                //優先度を変更
                _vCam0.Priority = 100;
                //他カメラの優先度をリセット
                _vCam1.Priority = 10;
                _CameraTest._vCam = _nowvCam;
                break;
            case 2:
                //現在のカメラを更新
                _nowvCam = _vCam1;
                //優先度を変更
                _vCam1.Priority = 100;
                //他カメラの優先度をリセット
                _vCam0.Priority = 10;
                _CameraTest._vCam = _nowvCam;
                break;
        }
    }
    
}
