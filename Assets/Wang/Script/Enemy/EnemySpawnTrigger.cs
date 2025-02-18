﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawnTrigger : MonoBehaviour
{
    [Header("複数のEnemySpawnController")]
    [SerializeField] private List<EnemySpawnController> enemySpawnControllers = new List<EnemySpawnController>();

    // トリガーに接触したら全てのEnemySpawnControllerでスポーンを開始
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("imouto") )
        {
            ActivateAllSpawns();
        }
    }

    // 全てのEnemySpawnControllerでスポーンを開始
    public void ActivateAllSpawns()
    {
        foreach (var spawnController in enemySpawnControllers)
        {
            if (spawnController != null)
            {
                spawnController._spawn = true; // スポーンを有効にする
                Debug.Log("Spawn triggered for: " + spawnController.gameObject.name);
            }
        }
    }

    public void ResetAllSpawn()
    {
        foreach (var spawnController in enemySpawnControllers)
        {
            if (spawnController != null)
            {
                spawnController._reset = true;
            }
        }
    }
}
