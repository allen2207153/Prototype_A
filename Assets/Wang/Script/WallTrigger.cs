using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public GameObject[] leftWalls; // 左側の壁の配列
    public GameObject[] rightWalls; // 右側の壁の配列
    public Vector3 moveDistance; // 壁の移動距離
    public float moveDuration = 1.0f; // 壁が移動する時間

    private Vector3[] initialLeftPositions; // 左側の各壁の初期位置
    private Vector3[] initialRightPositions; // 右側の各壁の初期位置

    void Start()
    {
        // 各壁の初期位置を記録
        initialLeftPositions = new Vector3[leftWalls.Length];
        initialRightPositions = new Vector3[rightWalls.Length];

        for (int i = 0; i < leftWalls.Length; i++)
        {
            initialLeftPositions[i] = leftWalls[i].transform.position;
        }

        for (int i = 0; i < rightWalls.Length; i++)
        {
            initialRightPositions[i] = rightWalls[i].transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがトリガーに触れた場合
        if (other.GetComponent<CharacterController>() != null && other.CompareTag("imouto"))
        {
            StartCoroutine(MoveWalls());
        }
    }

    IEnumerator MoveWalls()
    {
        // 各壁を同時に移動させる
        List<Coroutine> wallCoroutines = new List<Coroutine>();

        for (int i = 0; i < leftWalls.Length; i++)
        {
            // 左右の壁を同時に動かす
            Coroutine wallCoroutine = StartCoroutine(MoveWall(leftWalls[i], initialLeftPositions[i] + moveDistance, rightWalls[i], initialRightPositions[i] - moveDistance));
            wallCoroutines.Add(wallCoroutine);
        }

        // すべての壁の移動が完了するまで待つ
        foreach (Coroutine wallCoroutine in wallCoroutines)
        {
            yield return wallCoroutine;
        }
    }

   IEnumerator MoveWall(GameObject leftWall, Vector3 leftTargetPosition, GameObject rightWall, Vector3 rightTargetPosition)
    {
        Vector3 leftStartPosition = leftWall.transform.position;
        Vector3 rightStartPosition = rightWall.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsedTime / moveDuration); // 緩やかな加減速
            leftWall.transform.position = Vector3.Lerp(leftStartPosition, leftTargetPosition, t);
            rightWall.transform.position = Vector3.Lerp(rightStartPosition, rightTargetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leftWall.transform.position = leftTargetPosition;
        rightWall.transform.position = rightTargetPosition;
    }

    void OnDrawGizmosSelected()
    {
        // 壁の移動後の位置を表示
        Gizmos.color = Color.green;
        if (leftWalls != null && rightWalls != null)
        {
            for (int i = 0; i < leftWalls.Length; i++)
            {
                if (leftWalls[i] != null)
                {
                    Gizmos.DrawLine(leftWalls[i].transform.position, leftWalls[i].transform.position + moveDistance);
                }
                if (rightWalls[i] != null)
                {
                    Gizmos.DrawLine(rightWalls[i].transform.position, rightWalls[i].transform.position - moveDistance);
                }
            }
        }
    }
}