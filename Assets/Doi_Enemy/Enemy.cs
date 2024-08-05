using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Freeze
    };

    //パラメータ関数の定義
    public EnemyState _state; //キャラの状態
    private Transform _targetTransform; //ターゲットの情報
    private NavMeshAgent _navMeshAgent; //NavMeshAgentコンポーネント
    [SerializeField]
    private Vector3 destination; //目的地の位置情報を格納するためのパラメータ

    void Start()
    {
        //キャラのNavMeshAgentコンポーネントとnavMeshAgentを関連付ける
        _navMeshAgent = GetComponent<NavMeshAgent>();


        SetState(EnemyState.Idle); //初期状態をIdle状態に設定する
    }

    void Update()
    {
        //プレイヤーを目的地にして追跡する
        if (_state == EnemyState.Chase)
        {
            if (_targetTransform == null)
            {
                SetState(EnemyState.Idle);
            }
            else
            {
                SetDestination(_targetTransform.position);
                _navMeshAgent.SetDestination(GetDestination());
            }
            //　敵の向きをプレイヤーの方向に少しづつ変える
            var dir = (GetDestination() - transform.position).normalized;
            dir.y = 0;
            Quaternion setRotation = Quaternion.LookRotation(dir);
            //　算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, _navMeshAgent.angularSpeed * 0.1f * Time.deltaTime);
        }
    }

    //状態移行時に呼ばれる処理
    public void SetState(EnemyState tempState, Transform targetObject = null)
    {
        _state = tempState;

        if (tempState == EnemyState.Idle)
        {
            _navMeshAgent.isStopped = true; //キャラの移動を止める
        }
        else if (tempState == EnemyState.Chase)
        {
            _targetTransform = targetObject; //ターゲットの情報を更新
            _navMeshAgent.SetDestination(_targetTransform.position); //目的地をターゲットの位置に設定
            _navMeshAgent.isStopped = false; //キャラを動けるようにする
        }
        else if (tempState == EnemyState.Attack)
        {
            _navMeshAgent.isStopped = true; //キャラの移動を止める
        }
        else if (tempState == EnemyState.Freeze)
        {
            Invoke("ResetState", 2.0f);
        }
    }

    //　敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return _state;
    }

    //　目的地を設定する
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地を取得する
    public Vector3 GetDestination()
    {
        return destination;
    }

    private void ResetState()
    {
        SetState(EnemyState.Idle); ;
    }
}
