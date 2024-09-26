using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using static UnityEngine.GraphicsBuffer;

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
    public float _followDistance = 10f; // imoutoに近づいたら追跡する距離
    [Header("imoutoの前で停止する距離")]
    public float _stopDistance = 2.0f;  // imoutoの前で停止する距離
    [Header("imoutoの近くでIdleに変わる距離")]
    public float _idleDistance = 1.5f;  // Idle状態に移行する距離
    private bool _walkPointSet;         // 巡回位置が決定したかどうかの確認
    private Transform _targetTransform; // ターゲットの情報
    private Vector3 _destination;       // 目的地の位置情報を格納するためのパラメータ
    private GameObject _imouto;         // タグ "imouto" を持つターゲット

    void Start()
    {
        _walkPointSet = false;
        SetState(EnemyState.Patrol);

        //// "imouto" タグを持つオブジェクトを探す
        _imouto = GameObject.FindWithTag("imouto");
        //CharacterController imoutoController = GameObject.FindWithTag("imouto").GetComponent<CharacterController>();
        CharacterController playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        Collider enemyCollider = this.GetComponent<Collider>();
        Physics.IgnoreCollision(playerController, enemyCollider,true);

        //// CharacterControllerと敵のColliderの衝突を無視
        //Physics.IgnoreCollision(imoutoController, enemyCollider );
        //Physics.IgnoreCollision(playerController, enemyCollider);
    }

    void Update()
    {
        // もしimoutoが存在するなら、距離を計測
        if (_imouto != null)
        {
            float distanceToImouto = Vector3.Distance(transform.position, _imouto.transform.position);

            // imoutoが指定された距離以内にいる場合、追跡モードに切り替え
            if (distanceToImouto <= _followDistance && _state != EnemyState.Idle)
            {
                SetState(EnemyState.Chase, _imouto.transform);
            }

            // imoutoの前で停止し、Idle状態に移行
            if (_state == EnemyState.Chase)
            {
                if (distanceToImouto <= _stopDistance)
                {
                    if (distanceToImouto <= _idleDistance)
                    {
                        SetState(EnemyState.Idle);
                    }
                }
            }

            // imoutoが一定範囲から離れたらChase状態に戻す
            if (_state == EnemyState.Idle && distanceToImouto > _stopDistance)
            {
                SetState(EnemyState.Chase, _imouto.transform);
            }
        }

        if (_state == EnemyState.Patrol)
        {
            Patrol();
        }
        else if (_state == EnemyState.Chase || _state == EnemyState.Idle)
        {
            // Chase状態やIdle状態では、敵がimoutoに常に向く
            FaceImouto();
            if (_state == EnemyState.Chase)
            {
                Chase();
            }
        }
    }

    private void Patrol()
    {
        if (_walkPointSet)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, _patrolSpeed * Time.deltaTime);
            // 敵の向きを巡回ポイントの方向に少しずつ変える
            var dir = (GetDestination() - transform.position).normalized;
            dir.y = 0;
            Quaternion setRotation = Quaternion.LookRotation(dir);
            // 算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _angularSpeed * Time.deltaTime);
        }

        Vector3 distanceToWalkPoint = transform.position - _destination;
        // Walkpointに到着
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
        // imoutoの方向に向ける
        if (_imouto != null)
        {
            Vector3 direction = (_imouto.transform.position - transform.position).normalized;
            direction.y = 0; // 上下方向の回転を抑制
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
            // Idle時には動かず、ターゲットもリセット
            _targetTransform = null;
        }
        else if (tempState == EnemyState.Attack)
        {
            SetState(EnemyState.Freeze);
        }
        else if (tempState == EnemyState.Freeze)
        {
            Invoke("ResetChase", _arrivalFreezeTime);
        }
    }

    private void SearchWalkPoint()
    {
        if (_state == EnemyState.Patrol)
        {
            // ランダムでPatrol位置を決定
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

    private void ResetChase()
    {
        SetState(EnemyState.Chase, _targetTransform);
    }
}
