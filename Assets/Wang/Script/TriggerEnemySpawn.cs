using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab; // 敵のプレハブ
    public Transform spawnPoint; // 敵の生成位置
    public Transform[] targetPoints; // 敵の移動先の可能な位置
    public int numberOfEnemies = 5; // 生成する敵の数
    public float moveSpeed = 3.0f; // 敵の移動速度

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーによってトリガーされたか確認
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                SpawnAndMoveEnemy();
            }
        }
    }

    void SpawnAndMoveEnemy()
    {
        // 敵を生成
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // ランダムにターゲット位置を選択
        Transform targetPoint = targetPoints[Random.Range(0, targetPoints.Length)];

        // 敵を移動させる
        StartCoroutine(MoveEnemyToTargetAndDestroy(spawnedEnemy, targetPoint));
    }

    IEnumerator MoveEnemyToTargetAndDestroy(GameObject enemy, Transform targetPoint)
    {
        while (enemy != null && Vector3.Distance(enemy.transform.position, targetPoint.position) > 0.1f)
        {
            // 敵の向きを目標地点に向かせる
            Vector3 direction = (targetPoint.position - enemy.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

            // 敵を目標地点に向かって移動させる
            enemy.transform.position = Vector3.MoveTowards(
                enemy.transform.position,
                targetPoint.position,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        // 目標地点に到達した後、敵を削除
        if (enemy != null)
        {
            Destroy(enemy);
        }
    }
}
