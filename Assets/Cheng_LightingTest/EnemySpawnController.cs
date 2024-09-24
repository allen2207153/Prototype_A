using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawnController : MonoBehaviour
{
    [Header("エフェクト")]
    [SerializeField] private VisualEffect _spawnEffect;

    [Header("敵情報")]
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _raisingSpeed = 1.0f;
    [SerializeField] private Vector3 _resetPos = new (0.0f, -2.5f, 0.0f);
    [SerializeField] private float _delayTime = 0.0f;
    private Vector3 _enemyPos;


    [Header("Debug Option")]
    [SerializeField] private bool _spawn = false;
    [SerializeField] private bool _reset = false;

    private bool _spawnComplete = false;
    private float _delayCnt = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (_spawnEffect != null)
        {
            _spawnEffect.Stop();
        }

        if(_enemy != null)
        {
            //_enemyPos = _resetPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_spawn && !_spawnComplete)
        {
            Spawn();
        }

        if(_reset)
        {
            resetPos();
        }

    }

    private void Spawn()
    {
        if (_delayCnt <= 0)
        {
            _spawnEffect.Play();
        }
        _delayCnt++;
        
        if(_delayCnt * Time.deltaTime >= _delayTime)
        {
            _enemy.transform.localPosition = _enemyPos;
            _enemyPos.y += _raisingSpeed * Time.deltaTime;
        }
        
        
        if(_enemyPos.y >= 0.0f)
        {
            _spawnComplete = true;
            _spawn = false;
            //_spawnEffect.Stop();
            _delayCnt = 0.0f;
        }
        
    }

    private void resetPos()
    {
        _enemy.transform.localPosition = _resetPos;
        _enemyPos = _resetPos;
        _reset = false;
        _spawnComplete = false;
    }
}
