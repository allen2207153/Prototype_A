using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private KnockBack _knockBack;


    private float _damage;

    void Start()
    {
        _knockBack = FindObjectOfType<KnockBack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _knockBack.EnemyKnockBack(other.gameObject);
            EnemyHPSystem enemyHP = other.GetComponent<EnemyHPSystem>();
            _damage = GameObject.Find("Oniisan").GetComponent<MeleeAttack>()._damage;
            Debug.Log("AttackSuccesssss");
            enemyHP.ReceiveDamage(_damage);
            Debug.Log(_damage);
        }
    }
}
