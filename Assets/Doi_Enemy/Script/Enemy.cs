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
    [Header("追跡の移動速度速度")]
    public float _chaseSpeed;
    [Header("巡回の移動速度")]
    public float _patrolSpeed;
    [Header("方向転換する速度")]
    public float _angularSpeed;
    [Header("巡回ポイント到着後の硬直時間")]
    public float _arrivalFreezeTime;
    [Header("攻撃終了後の硬直時間")]
    public float _attackFreezeTime;
    private bool _walkPointSet; //巡回位置が決定したかどうかの確認
    private Transform _targetTransform; //ターゲットの情報
    private Vector3 _destination; //目的地の位置情報を格納するためのパラメータ
    // Start is called before the first frame update
    void Start()
    {
        _walkPointSet = false;
        SetState(EnemyState.Patrol);
    }


    // Update is called once per frame
    void Update()
    {
        if (_state == EnemyState.Patrol)
        {
            if (_walkPointSet)
            {
                transform.position = Vector3.MoveTowards(transform.position, _destination, _patrolSpeed * Time.deltaTime);
                //　敵の向きをプレイヤーの方向に少しづつ変える
                var dir = (GetDestination() - transform.position).normalized;
                dir.y = 0;
                Quaternion setRotation = Quaternion.LookRotation(dir);
                //　算出した方向の角度を敵の角度に設定
                transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _angularSpeed * Time.deltaTime);
            }
            Vector3 distanceToWalkPoint = transform.position - _destination;
            //Walkpointに到着
            if (distanceToWalkPoint.magnitude < 1f && _walkPointSet)
            {
                _walkPointSet = false;
                Invoke("SearchWalkPoint", _attackFreezeTime);
            }
        }
        else if (_state == EnemyState.Chase)
        {
            if (_targetTransform == null)
            {
                SetState(EnemyState.Patrol);
            }
            else
            {
                SetDestination(_targetTransform.position);
                transform.position = Vector3.MoveTowards(transform.position, _destination, _chaseSpeed * Time.deltaTime);
                //　敵の向きをプレイヤーの方向に少しづつ変える
                var dir = (GetDestination() - transform.position).normalized;
                dir.y = 0;
                Quaternion setRotation = Quaternion.LookRotation(dir);
                //　算出した方向の角度を敵の角度に設定
                transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _angularSpeed * Time.deltaTime);
            }
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
            _targetTransform = targetObject; //ターゲットの情報を更新
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
            //ランダムでPatrol位置を決定
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
