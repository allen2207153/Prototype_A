using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 50f;
    [SerializeField] private GameObject _enemy;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            _enemy = collision.gameObject;
            EnemyKnockBack();
        }
    }

    void EnemyKnockBack()
    {
        _enemy.transform.position += transform.forward * Time.deltaTime * _knockBackForce;
    }
}
