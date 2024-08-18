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
        Chase,
        Attack,
        Freeze,
        Patrol
    };

    //パラメータ関数の定義
    public EnemyState _state; //キャラの状態
    private Transform _targetTransform; //ターゲットの情報
    private NavMeshAgent _navMeshAgent; //NavMeshAgentコンポーネント
    public float _timeBetweenAttacks;//攻撃の間隔
    bool _alreadyAttacked;//攻撃終了したかの確認
    bool walkPointSet;//patrol目的地が決定したかの確認
    public float walkPointRange;//patrol目的地の範囲
    [SerializeField]
    private Vector3 _destination; //目的地の位置情報を格納するためのパラメータ
    public LayerMask _whatIsGround;//navmeshが適応されている地面



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
        else if(_state==EnemyState.Attack)
        {
            if (!_alreadyAttacked)
            {
                ///攻撃の処理をここに

                _alreadyAttacked = true;
                Invoke(nameof(ResetAttack), _timeBetweenAttacks);
            }
            _state = EnemyState.Idle;
        }
        else if(_state==EnemyState.Patrol)
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
                _navMeshAgent.SetDestination(_destination);

            Vector3 distanceToWalkPoint = transform.position - _destination;

            //目的地到着
            if (distanceToWalkPoint.magnitude < 1f)
                walkPointSet = false;

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
        else if(tempState==EnemyState.Patrol)
        {
            _navMeshAgent.isStopped = false; //キャラを動けるようにする
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        _destination = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(_destination, -transform.up, 2f, _whatIsGround))//navmeshで歩ける場所か確認
        {
            walkPointSet = true;
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
        _destination = position;
    }

    //　目的地を取得する
    public Vector3 GetDestination()
    {
        return _destination;
    }

    private void ResetState()
    {
        SetState(EnemyState.Idle); ;
    }
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
}
