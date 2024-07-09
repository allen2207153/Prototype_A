using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraChenge : MonoBehaviour
{
    [Header("ブレンドしたいカメラ")]
    [SerializeField] private GameObject _chi1;
    [SerializeField] private GameObject _chi2;

    [Header("現在のカメラ")]
    public GameObject _Rchi;
    // Start is called before the first frame update
    void Start()
    {
        _Rchi = _chi1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.U))
        {
            if(_chi1.activeInHierarchy)
            {
                _chi1.SetActive(false);
                Cange(2);
            }
            else
            {
                _chi1.SetActive(true);
                Cange(1);
            }
        }
    }

    public void Cange(int c)
    {
        PlayerMovement_CameraTest _CameraTest = GetComponent<PlayerMovement_CameraTest>();
        
        switch (c)
        {
            case 1:
                _Rchi = _chi1;
                _CameraTest._vCam = _Rchi;
                break;
            case 2:
                _Rchi = _chi2;
                _CameraTest._vCam = _Rchi;
                break;
        }
    }

}
