using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySencer : MonoBehaviour
{
    [SerializeField]
    private SphereCollider _searchArea = default;
    [SerializeField]
    private float _searchAngle = 45f;
    private LayerMask _obstacleLayer = default;
    private Enemy _enemyMove = default;

    private void Start()
    {
        _enemyMove = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerStay(Collider target)
    {

        if (target.tag == "Player")
        {
            Debug.Log("SencerPlayer検知");
            var playerDirection = target.transform.position - transform.position;

            var angle = Vector3.Angle(transform.forward, playerDirection);

            if (angle <= _searchAngle)
            {
                //obstacleLayer = LayerMask.GetMask("Block", "Wall");

                if (!Physics.Linecast(transform.position + Vector3.up, target.transform.position + Vector3.up, _obstacleLayer))　//プレイヤーとの間に障害物がないとき
                {
                    if (Vector3.Distance(target.transform.position, transform.position) <= _searchArea.radius * 0.5f
                        && Vector3.Distance(target.transform.position, transform.position) >= _searchArea.radius * 0.05f)
                    {
                        Debug.Log("Attackセンサー実行");
                        _enemyMove.SetState(Enemy.EnemyState.Attack);
                    }
                    else if (Vector3.Distance(target.transform.position, transform.position) <= _searchArea.radius
                        && Vector3.Distance(target.transform.position, transform.position) >= _searchArea.radius * 0.5f
                        && _enemyMove._state == Enemy.EnemyState.Patrol)
                    {
                        Debug.Log("Chaseセンサー実行");
                        _enemyMove.SetState(Enemy.EnemyState.Chase, target.transform); // センサーに入ったプレイヤーをターゲットに設定して、追跡状態に移行する。
                    }
                }
            }
            else if (angle > _searchAngle)
            {
                _enemyMove.SetState(Enemy.EnemyState.Patrol);
            }
        }
    }
#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -_searchAngle, 0f) * transform.forward, _searchAngle * 2f, _searchArea.radius);
    }
#endif
}
