using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool _walkPointSet;
    private Transform _targetTransform;
    private Vector3 _destination;
    private GameObject _imouto;
    private Light _pointLight; // 光源

    void Start()
    {
        _walkPointSet = false;
        SetState(EnemyState.Patrol);

        // "imouto" タグを持つオブジェクトを探す
        _imouto = GameObject.FindWithTag("imouto");

        // 添加头顶的Point Light
        GameObject lightObj = new GameObject("EnemyPointLight");
        lightObj.transform.parent = transform;
        lightObj.transform.localPosition = new Vector3(0, 2, 0); // 光源位置稍微高于敌人
        _pointLight = lightObj.AddComponent<Light>();
        _pointLight.type = LightType.Point;
        _pointLight.range = _lightRadius;
        _pointLight.intensity = 1.5f;
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
    }
    

    private void HandleLightDetection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _lightRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == _imouto)
            {
                // 让imouto向敌人移动
                Vector3 directionToEnemy = (transform.position - _imouto.transform.position).normalized;
                _imouto.transform.position += directionToEnemy * _imoutoMoveSpeed * Time.deltaTime;
            }
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
            Vector3 direction = (_imouto.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _angularSpeed * Time.deltaTime);
        }
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
}
