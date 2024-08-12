using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 300f;
    [SerializeField] private GameObject _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _enemy = other.gameObject;
            EnemyKnockBack();
        }
    }

    void EnemyKnockBack()
    {
        //_enemy.transform.position += transform.forward * Time.deltaTime * _knockBackForce;
        _enemy.GetComponent<Rigidbody>().AddForce(transform.forward * _knockBackForce);
    }
}
