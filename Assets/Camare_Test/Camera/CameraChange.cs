using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

[Serializable]
public class VCamDate
{
    public CinemachineVirtualCamera _vCam;
}


public class CameraChange : MonoBehaviour
{
    [Header("ブレンドしたいカメラ")]
    [SerializeField] private CinemachineVirtualCamera _vCam0;
    [SerializeField] private CinemachineVirtualCamera _vCam1;
    [SerializeField] private CinemachineVirtualCamera _vCam2;
    [SerializeField] private CinemachineVirtualCamera _vCam3;
    [SerializeField] private CinemachineVirtualCamera _vCam4;
    [SerializeField] private CinemachineVirtualCamera _vCam5;
    [SerializeField] private CinemachineVirtualCamera _vCam6;
    [SerializeField] private CinemachineVirtualCamera _vCam7;
    
    [SerializeField]
    VCamDate[] _vCamList;
    [Header("PlayerMovementスクリプト")]
    [SerializeField] private PlayerMovement_CameraTest _CameraTest;

    [Header("現在のカメラ")]
    public CinemachineVirtualCamera _nowvCam;

    [Header("優先度の値")]
    [SerializeField] private int _hPriority = 100;//高い優先度
    [SerializeField] private int _rPriority = 5;//低い優先度

    // Start is called before the first frame update
    void Start()
    {
        _nowvCam = _vCam0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change(int c)
    {
        
        switch (c)
        {
            case 0:
                //現在のカメラを更新
                _nowvCam = _vCam0;
                //優先度を変更
                _vCam0.Priority = _hPriority;
                //他カメラの優先度をリセット
                _vCam1.Priority = _rPriority;
                /*追加したら対応しているPriorityとcaseを解除
                _vCam2.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                */
                //
                _CameraTest._vCam = _nowvCam;
                break;
            case 1:
                //現在のカメラを更新
                _nowvCam = _vCam1;
                //優先度を変更
                _vCam1.Priority = _hPriority;
                //他カメラの優先度をリセット
                _vCam0.Priority = _rPriority;
                /*追加したら対応しているPriorityとcaseを解除
                _vCam2.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                */
                //
                _CameraTest._vCam = _nowvCam;
                break;
                /*
            case 2:
                //現在のカメラを更新
                _nowvCam = _vCam2;
                //優先度を変更
                _vCam2.Priority = _hPriority;
                //他カメラの優先度をリセット
                追加したら対応しているPriorityとcaseを解除
                _vCam0.Priority = _rPriority;
                _vCam1.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                //
                _CameraTest._vCam = _nowvCam;
                break;
            case 3:
                //現在のカメラを更新
                _nowvCam = _vCam3;
                //優先度を変更
                _vCam3.Priority = _hPriority;
                //他カメラの優先度をリセット
                追加したら対応しているPriorityとcaseを解除
                _vCam0.Priority = _rPriority;
                _vCam1.Priority = _rPriority;
                _vCam2.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                //
                _CameraTest._vCam = _nowvCam;
                break;
            case 4:
                //現在のカメラを更新
                _nowvCam = _vCam4;
                //優先度を変更
                _vCam4.Priority = _hPriority;
                //他カメラの優先度をリセット
                追加したら対応しているPriorityとcaseを解除
                _vCam0.Priority = _rPriority;
                _vCam1.Priority = _rPriority;
                _vCam2.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                //
                _CameraTest._vCam = _nowvCam;
                break;
            case 5:
                //現在のカメラを更新
                _nowvCam = _vCam5;
                //優先度を変更
                _vCam5.Priority = _hPriority;
                //他カメラの優先度をリセット
                追加したら対応しているPriorityとcaseを解除
                _vCam0.Priority = _rPriority;
                _vCam1.Priority = _rPriority;
                _vCam2.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                //
                _CameraTest._vCam = _nowvCam;
                break;
            case 6:
                //現在のカメラを更新
                _nowvCam = _vCam6;
                //優先度を変更
                _vCam6.Priority = _hPriority;
                //他カメラの優先度をリセット
                追加したら対応しているPriorityとcaseを解除
                _vCam0.Priority = _rPriority;
                _vCam1.Priority = _rPriority;
                _vCam2.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam7.Priority = _rPriority;
                //
                _CameraTest._vCam = _nowvCam;
                break;
            case 7:
                //現在のカメラを更新
                _nowvCam = _vCam7;
                //優先度を変更
                _vCam7.Priority = _hPriority;
                //他カメラの優先度をリセット
                追加したら対応しているPriorityとcaseを解除
                _vCam0.Priority = _rPriority;
                _vCam1.Priority = _rPriority;
                _vCam2.Priority = _rPriority;
                _vCam3.Priority = _rPriority;
                _vCam4.Priority = _rPriority;
                _vCam5.Priority = _rPriority;
                _vCam6.Priority = _rPriority;
                //
                _CameraTest._vCam = _nowvCam;
                break;
                */

        }
    }
    
}
