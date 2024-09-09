using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    private KnockBack _knockBack;

    void Start()
    {
        _knockBack = FindObjectOfType<KnockBack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _knockBack.EnemyKnockBack(other.gameObject);
        }
    }
}
