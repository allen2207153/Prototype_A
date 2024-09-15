using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPSystem : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth = 100f;
    [SerializeField]
    private float _maxHealth = 100f;

    public void ReceiveDamage(float damage)
    {
        float tmp = _currentHealth;
        tmp -= damage;

        if (tmp <= 0)
        {
            _currentHealth = 0;
            Destroy(gameObject);
        }
        else
        {
            _currentHealth = tmp;
        }
    }
}
