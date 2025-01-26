using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Freeze
    };

    [Header("キャラの状態")]
    public EnemyState _state;
    [Header("現在地から巡回ポイントまでの捜索範囲")]
    public float _walkPointRange;
    [Header("追跡の移動速度")]
    public float _chaseSpeed;
    [Header("巡回の移動速度")]
    public float _patrolSpeed;
    [Header("方向転換する速度")]
    public float _angularSpeed;
    [Header("巡回ポイント到着後の硬直時間")]
    public float _arrivalFreezeTime;
    [Header("攻撃終了後の硬直時間")]
    public float _attackFreezeTime;
    [Header("imoutoを追跡する距離")]
    public float _followDistance = 10f;
    [Header("imoutoの前で停止する距離")]
    public float _stopDistance = 2.0f;
    [Header("imoutoの近くでIdleに変わる距離")]
    public float _idleDistance = 1.5f;
    [Header("光照範囲")]
    public float _lightRadius = 5f; // 光的照射半径
    [Header("imouto移動速度")]
    public float _imoutoMoveSpeed = 3f;
    [Header("Spotlightの最小強度")]
    public float _minSpotlightIntensity = 1.0f; // Spotlight 的最小强度
    [Header("Spotlightの最大強度")]
    public float _maxSpotlightIntensity = 3.0f; // Spotlight 的最大强度
    [Header("Spotlight強度変化速度")]
    public float _spotlightIntensityChangeSpeed = 0.5f; // Spotlight 强度变化速度

    private bool _walkPointSet;
    private Transform _targetTransform;
    private Vector3 _destination;
    private GameObject _imouto;
    private Animator _imoutoAnimator; // imoutoのAnimator
    private Light _pointLight; // 光源
    private float intensityTimer = 0.0f;


    public Light _spotLight; // スポットライト
    public RoomManager roomManager; // RoomManager の参照
    public GameObject player; // プレイヤー
    public GameObject npc; // NPC
    public float maxIntensityDuration = 3.0f;


    public float vibrationIntensity = 0.5f; // 手柄振動强度（0.0 ~ 1.0）
    public float vibrationDuration = 0.1f; // 手柄振動時間
    private float currentVibrationTime = 0f;

    void Start()
    {
        _walkPointSet = false;
        SetState(EnemyState.Patrol);

        // "imouto" タグを持つオブジェクトを探す
        _imouto = GameObject.FindWithTag("imouto");
        if (_imouto != null)
        {
            _imoutoAnimator = _imouto.GetComponent<Animator>(); // Animatorを取得
        }

        // 添加头顶的Point Light
        GameObject lightObj = new GameObject("EnemyPointLight");
        lightObj.transform.parent = transform;
        lightObj.transform.localPosition = new Vector3(0, 2, 0); // 光源位置稍微高于敌人
        _pointLight = lightObj.AddComponent<Light>();
        _pointLight.type = UnityEngine.LightType.Point;
        _pointLight.range = _lightRadius;
        _pointLight.intensity = 0.1f;
    }

    void Update()
    {
        // もしimoutoが存在するなら、距離を計測
        if (_imouto != null)
        {
            float distanceToImouto = Vector3.Distance(transform.position, _imouto.transform.position);

            // 距離が十分近い場合、Idle状態に切り替え
            if (distanceToImouto <= _idleDistance)
            {
                SetState(EnemyState.Idle);
            }
            else if (_state == EnemyState.Idle && distanceToImouto > _idleDistance && distanceToImouto <= _followDistance)
            {
                // IdleからChaseに切り替え
                SetState(EnemyState.Chase, _imouto.transform);
            }
            else if (_state != EnemyState.Idle && distanceToImouto <= _followDistance)
            {
                // imoutoが追跡距離に入った場合、Chase状態に切り替え
                SetState(EnemyState.Chase, _imouto.transform);
            }
        }

        // 状態ごとの動作
        if (_state == EnemyState.Patrol)
        {
            Patrol();
        }
        else if (_state == EnemyState.Chase)
        {
            Chase();
            FaceImouto();
            HandleLightDetection();
        }
        else if (_state == EnemyState.Idle)
        {
            FaceImouto(); // Idleでもimoutoの方向を向く
        }

        // Spotlightの強度を調整
        HandleSpotlightIntensity();
        Debug.Log(intensityTimer);
        // 光の範囲内にimoutoがいるか確認
        if (IsImoutoInLight() && _state == EnemyState.Idle)
        {
            // Spotlightの強度を増加させる
            _spotLight.intensity = Mathf.MoveTowards(_spotLight.intensity, _maxSpotlightIntensity, _spotlightIntensityChangeSpeed * Time.deltaTime);

            // 最大強度に達した場合、タイマーを開始
            if (Mathf.Approximately(_spotLight.intensity, _maxSpotlightIntensity))
            {
                intensityTimer += Time.deltaTime;

                // 最大強度が指定時間維持された場合、RoomManagerのリセット処理を実行
                if (intensityTimer >= maxIntensityDuration)
                {
                    roomManager.TriggerReset(player, npc);
                }
            }
        }
        else
        {
            // Spotlightの強度をリセットし、タイマーをリセット
            _spotLight.intensity = Mathf.MoveTowards(_spotLight.intensity, 0, _spotlightIntensityChangeSpeed * Time.deltaTime);
            intensityTimer = 0.0f;
        }

        // 光範囲内のimoutoを検知して移動させる処理
    }
    private void HandleLightDetection()
    {
        bool isInLightRange = false; // imoutoが光範囲内にいるかどうか
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _lightRadius); // 光範囲内のコライダーを取得
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == _imouto)
            {
                isInLightRange = true;
                // imoutoを敵の方向に移動させる
                Vector3 directionToEnemy = (transform.position - _imouto.transform.position).normalized;
                _imouto.transform.position += directionToEnemy * _imoutoMoveSpeed * Time.deltaTime;

                // imoutoを敵の方向に向かせる
                FaceTarget(_imouto.transform, transform.position);

                // Animatorのパラメータを更新
                if (_imoutoAnimator != null)
                {
                    _imoutoAnimator.SetBool("isWalking", true); // 歩行アニメーションを再生
                }
            }
        }

        // 光範囲外の場合、アニメーションパラメータをリセット
        if (!isInLightRange && _imoutoAnimator != null)
        {
            _imoutoAnimator.SetBool("isWalking", false); // 歩行アニメーションを停止
        }
    }

    // Spotlightの強度を調整
    private void HandleSpotlightIntensity()
    {
        if (_spotLight == null) return; // Spotlightが存在しない場合、処理を終了

        bool isImoutoInLight = false; // imoutoが光の範囲内にいるかどうか
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _lightRadius); // 光範囲内のコライダーを取得
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == _imouto && _state == EnemyState.Idle)
            {
                isImoutoInLight = true;
                break; // imoutoが検知された場合、ループを終了
            }
        }

        // 判定結果に基づいてSpotlightの強度を調整
        if (isImoutoInLight)
        {
            _spotLight.intensity = Mathf.MoveTowards(_spotLight.intensity, _maxSpotlightIntensity, _spotlightIntensityChangeSpeed * Time.deltaTime);
            // 光強度に応じて手柄の震動を調整
            TriggerVibration(_spotLight.intensity / _maxSpotlightIntensity);
        }
        else
        {
            _spotLight.intensity = Mathf.MoveTowards(_spotLight.intensity, _minSpotlightIntensity, _spotlightIntensityChangeSpeed * Time.deltaTime);
            TriggerVibration(0); // 手柄の震動を停止
        }
    }
    private void Patrol()
    {
        if (_walkPointSet)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, _patrolSpeed * Time.deltaTime);
            var dir = (GetDestination() - transform.position).normalized;
            dir.y = 0;
            Quaternion setRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _angularSpeed * Time.deltaTime);
        }

        Vector3 distanceToWalkPoint = transform.position - _destination;
        if (distanceToWalkPoint.magnitude < 1f && _walkPointSet)
        {
            _walkPointSet = false;
            Invoke("SearchWalkPoint", _arrivalFreezeTime);
        }
    }

    private void Chase()
    {
        if (_targetTransform == null)
        {
            SetState(EnemyState.Patrol);
        }
        else
        {
            SetDestination(_targetTransform.position);
            transform.position = Vector3.MoveTowards(transform.position, _destination, _chaseSpeed * Time.deltaTime);
        }
    }

    private void FaceImouto()
    {
        if (_imouto != null)
        {
            FaceTarget(transform, _imouto.transform.position);
        }
    }

    private void FaceTarget(Transform source, Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - source.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        source.rotation = Quaternion.Slerp(source.rotation, targetRotation, _angularSpeed * Time.deltaTime);
    }

    public void SetState(EnemyState tempState, Transform targetObject = null)
    {
        _state = tempState;

        if (tempState == EnemyState.Patrol)
        {
            SearchWalkPoint();
        }
        else if (tempState == EnemyState.Chase)
        {
            _targetTransform = targetObject; // ターゲットの情報を更新
        }
        else if (tempState == EnemyState.Idle)
        {
            _targetTransform = null; // Idle時にはターゲットもリセット
        }
    }

    private void SearchWalkPoint()
    {
        if (_state == EnemyState.Patrol)
        {
            float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
            float randomX = Random.Range(-_walkPointRange, _walkPointRange);

            _destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            _walkPointSet = true;
        }
    }

    public void SetDestination(Vector3 position)
    {
        _destination = position;
    }

    public Vector3 GetDestination()
    {
        return _destination;
    }

    private bool IsImoutoInLight()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _lightRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("imouto"))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 手柄の震動をトリガーする。
    /// 光強度に基づき、震動の強度を比例計算して適用。
    /// </summary>
    /// <param name="normalizedIntensity">光強度の正規化値（0.0～1.0）</param>
    private void TriggerVibration(float normalizedIntensity)
    {
        // 手柄が接続されている場合のみ処理を行う
        if (Gamepad.current != null)
        {
            // 現在の震動タイマーを増加
            currentVibrationTime += Time.deltaTime;

            // タイマーが震動の間隔を超えた場合、震動を発生させる
            if (currentVibrationTime >= vibrationDuration)
            {
                // 光強度に基づき、左モーターと右モーターの震動強度を設定
                Gamepad.current.SetMotorSpeeds(normalizedIntensity * vibrationIntensity, normalizedIntensity * vibrationIntensity);

                // タイマーをリセット
                currentVibrationTime = 0f;
            }
        }
    }

    /// <summary>
    /// 手柄の震動を停止する。
    /// モーターの震動強度をゼロに設定して、震動を完全にオフにする。
    /// </summary>
    public void StopVibration()
    {
        // 手柄が接続されている場合のみ処理を行う
        if (Gamepad.current != null)
        {
            // 左モーターと右モーターの震動を停止
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }

}
