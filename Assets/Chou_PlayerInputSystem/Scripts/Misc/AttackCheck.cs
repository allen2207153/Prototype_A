using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private KnockBack _knockBack;

    private EnemyHPSystem _enemyHPSystem;

    private float _damage;

    void Start()
    {
        _knockBack = FindObjectOfType<KnockBack>();
        _enemyHPSystem = FindObjectOfType<EnemyHPSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _knockBack.EnemyKnockBack(other.gameObject);

            _damage = GameObject.Find("Oniisan").GetComponent<MeleeAttack>()._damage;
            _enemyHPSystem.ReceiveDamage(_damage);
        }
    }
}
