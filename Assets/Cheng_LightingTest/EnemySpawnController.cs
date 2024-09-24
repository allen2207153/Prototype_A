using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawnController : MonoBehaviour
{
    [Header("エフェクト")]
    [SerializeField] public VisualEffect _spawnEffect; // スポーン時のエフェクト
    [SerializeField] public VisualEffect _deathEffect; // 死亡時のエフェクト

    [Header("敵情報")]
    [SerializeField] private float _raisingSpeed = 1.0f;
    [SerializeField] private Vector3 _resetPos = new Vector3(0.0f, -2.5f, 0.0f); // リセット時の位置
    [SerializeField] private float _delayTime = 0.0f; // スポーン遅延
    private Vector3 _enemyPos;

    [Header("消滅処理")]
    [SerializeField] private float _sinkSpeed = 1.0f; // 敵が沈む速度
    private bool _sinkComplete = false; // 沈みが完了したかどうか
    private Vector3 _initialPosition; // 敵が死亡時の位置

    [Header("Debug Option")]
    public bool _spawn = false;
    public bool _reset = false;

    private float _delayCnt = 0.0f;

    // 敵のコンポーネントへの参照
    private CapsuleCollider _enemyCollider;
    private Rigidbody _enemyRigidbody;
    private Enemy _enemyScript;
    private EnemyHPSystem _enemyHPSystem;

    void Start()
    {
        _enemyPos = _resetPos;
        _initialPosition = transform.localPosition; // 敵の初期位置を記録

        _enemyCollider = GetComponent<CapsuleCollider>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _enemyScript = GetComponent<Enemy>();
        _enemyHPSystem = GetComponent<EnemyHPSystem>();

        DisableEnemy(); // スタート時に敵を無効化

        if (_spawnEffect != null) _spawnEffect.Stop();
        if (_deathEffect != null) _deathEffect.Stop();
    }

    void Update()
    {
      

        if (_spawn)
        {
            Spawn();
        }

        if (_reset)
        {
            ResetPosition();
        }

        if (_enemyHPSystem._isDead && !_sinkComplete)
        {
            SinkAndReset(); // 敵を沈めてリセット
        }
    }

    // スポーン処理
    private void Spawn()
    {
        if (_delayCnt <= 0)
        {
            _spawnEffect.Play(); // スポーンエフェクトを再生
        }

        _delayCnt++;

        if (_delayCnt * Time.deltaTime >= _delayTime)
        {
            transform.localPosition = _enemyPos;
            _enemyPos.y += _raisingSpeed * Time.deltaTime;
        }

        if (_enemyPos.y >= 0.0f)
        {
            _spawnEffect.Stop(); // スポーンエフェクト停止
            _delayCnt = 0.0f;

            EnableEnemy(); // 敵のコンポーネントを有効にする
        }
    }

    // 敵の状態をリセットする処理
    private void ResetPosition()
    {
        transform.localPosition = _resetPos; // 親に対してのローカル位置をリセット
        _enemyPos = _resetPos;
        _reset = false;
        _spawn = false;

        DisableEnemy(); // 再び敵のコンポーネントを無効化
    }

    // 敵を沈めてリセットする処理
    public void SinkAndReset()
    {
        DisableEnemy();
       // _deathEffect.Play();

        Vector3 currentPosition = transform.localPosition;
        currentPosition.y -= _sinkSpeed * Time.deltaTime;
        transform.localPosition = currentPosition;

        if (transform.localPosition.y <= _initialPosition.y - 2.5f)
        {
            _sinkComplete = true;
            ResetPosition();
            _enemyHPSystem.resetHealth();
            _spawn = false;
        }
    }

    // 敵のコンポーネントを有効化する
    private void EnableEnemy()
    {
        if (_enemyCollider != null) _enemyCollider.enabled = true;
        if (_enemyRigidbody != null)
        {
            _enemyRigidbody.isKinematic = false;
            _enemyRigidbody.useGravity = true;
        }
        if (_enemyScript != null) _enemyScript.enabled = true;
        if (_enemyHPSystem != null) _enemyHPSystem.enabled = true;
    }

    // 敵のコンポーネントを無効化する
    private void DisableEnemy()
    {
        if (_enemyCollider != null) _enemyCollider.enabled = false;
        if (_enemyRigidbody != null)
        {
            _enemyRigidbody.isKinematic = true;
            _enemyRigidbody.useGravity = false;
        }
        if (_enemyScript != null) _enemyScript.enabled = false;
        if (_enemyHPSystem != null) _enemyHPSystem.enabled = false;
    }
}