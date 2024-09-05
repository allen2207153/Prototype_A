using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public GameObject[] leftWalls; // 左側の壁の配列
    public Vector3 moveDistance; // 壁の移動距離
    public float moveDuration = 1.0f; // 壁が移動する時間
    public float delayBetweenMoves = 0.5f; // 壁が移動する間の時間差

    private Vector3[] initialPositions; // 各壁の初期位置を記録する配列

    void Start()
    {
        // 各壁の初期位置を記録
        initialPositions = new Vector3[leftWalls.Length];
        for (int i = 0; i < leftWalls.Length; i++)
        {
            initialPositions[i] = leftWalls[i].transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに触れた場合
        if (other.GetComponent<CharacterController>() != null)
        {
            StartCoroutine(MoveWalls());
        }
    }

    System.Collections.IEnumerator MoveWalls()
    {
        // 各壁を順番に移動させる
        for (int i = 0; i < leftWalls.Length; i++)
        {
            yield return StartCoroutine(MoveWall(leftWalls[i], initialPositions[i] + moveDistance));

            // 次の壁を移動させる前に時間差を待つ
            yield return new WaitForSeconds(delayBetweenMoves);
        }
    }

    System.Collections.IEnumerator MoveWall(GameObject wall, Vector3 targetPosition)
    {
        Vector3 startPosition = wall.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            wall.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wall.transform.position = targetPosition;
    }

    void OnDrawGizmosSelected()
    {
        // 壁の移動後の位置を表示
        Gizmos.color = Color.green;
        if (leftWalls != null)
        {
            for (int i = 0; i < leftWalls.Length; i++)
            {
                if (leftWalls[i] != null)
                {
                    Gizmos.DrawLine(leftWalls[i].transform.position, leftWalls[i].transform.position + moveDistance);
                }
            }
        }
    }
}
