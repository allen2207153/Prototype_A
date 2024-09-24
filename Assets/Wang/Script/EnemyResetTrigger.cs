using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResetTrigger : MonoBehaviour
{
    [Header("複数のEnemySpawnController")]
    [SerializeField] private List<EnemySpawnController> enemySpawnControllers = new List<EnemySpawnController>();

    // トリガーに接触したら全てのEnemySpawnControllerでスポーンを開始
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("imouto"))
        {
            ActivateAllSpawns();
        }
    }

    // 全てのEnemySpawnControllerでスポーンを開始
    private void ActivateAllSpawns()
    {
        foreach (var spawnController in enemySpawnControllers)
        {
            if (spawnController != null)
            {
                spawnController._reset = true; // スポーンを有効にする
                Debug.Log("Spawn triggered for: " + spawnController.gameObject.name);
            }
        }
    }
}
