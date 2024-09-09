using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Rate_Controller : MonoBehaviour
{
    [SerializeField] private VisualEffect _vfx;
    [SerializeField] private float _spawnRate = 10.0f;

    

    private void Start()
    {
    }

    void Update()
    {
        if (_vfx != null)
        {
            _vfx.playRate = _spawnRate;
        }


    }



}
