using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPSystem : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth = 100f;
    [SerializeField]
    private float _maxHealth = 100f;

    [Header("エフェクトコントローラー")]
    public EnemySpawnController _spawnController; // EnemySpawnControllerを参照

    [Header("消滅処理")]
    [SerializeField] private float _sinkSpeed = 1.0f; // 敵が沈む速度
    public bool _isDead = false;


    void Start()
    {
        setHealth(_maxHealth);
    }


    public void ReceiveDamage(float damage)
    {
        

        _currentHealth -= damage;
        Debug.Log("GetHurt");

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
        }
    }

   

    public void setHealth(float health)
    {
        _currentHealth = health;
    }

    public void resetHealth()
    {
        _currentHealth = _maxHealth;
        _isDead=false;
    }
}
