using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float _knockBackForce = 300f;
    [SerializeField] private GameObject _enemy;

    public void EnemyKnockBack(GameObject enemy)
    {
        // enemyタグを持っているか確認
        if (enemy.CompareTag("Enemy"))
        {
            _enemy = enemy;

            // Rigidbodyを取得して、ノックバック方向に力を加える
            Rigidbody enemyRigidbody = _enemy.GetComponent<Rigidbody>();

            if (enemyRigidbody != null)
            {
                // 前方方向にノックバックの力を加える
                enemyRigidbody.AddForce(transform.forward * _knockBackForce);
            }
        }
    }
}
