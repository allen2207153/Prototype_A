using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairController : MonoBehaviour
{
    public Transform[] stairSteps; // 各段を格納する配列
    public Vector3 moveDistance = new Vector3(0, 1.0f, 0); // 階段の伸縮量
    public float moveDuration = 0.5f; // 1段が移動するのにかかる時間
    public float delayBetweenSteps = 0.2f; // 次の段が動くまでの遅延
    public bool isExpanded = false; // 階段が展開されているかどうかの状態

    private Vector3[] initialPositions; // 各段の初期位置

    void Start()
    {
        // 各段の初期位置を保存
        initialPositions = new Vector3[stairSteps.Length];
        for (int i = 0; i < stairSteps.Length; i++)
        {
            initialPositions[i] = stairSteps[i].position;
        }
    }

    void Update()
    {
        // スペースキーで伸縮をトグル
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isExpanded)
            {
                StartCoroutine(CollapseStairs()); // 収縮
            }
            else
            {
                StartCoroutine(ExpandStairs()); // 展開
            }
            isExpanded = !isExpanded;
        }
    }

    IEnumerator ExpandStairs()
    {
        // 階段を1段ずつ展開
        for (int i = 0; i < stairSteps.Length; i++)
        {
            Vector3 targetPosition = initialPositions[i] + moveDistance * i;
            yield return StartCoroutine(MoveStep(stairSteps[i], targetPosition));

            // 次の段が動くまで少し待機
            yield return new WaitForSeconds(delayBetweenSteps);
        }
    }

    IEnumerator CollapseStairs()
    {
        // 階段を1段ずつ元の位置に戻す
        for (int i = stairSteps.Length - 1; i >= 0; i--)
        {
            yield return StartCoroutine(MoveStep(stairSteps[i], initialPositions[i]));

            // 次の段が動くまで少し待機
            yield return new WaitForSeconds(delayBetweenSteps);
        }
    }

    IEnumerator MoveStep(Transform step, Vector3 targetPosition)
    {
        Vector3 startPosition = step.position;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            step.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        step.position = targetPosition;
    }
}
