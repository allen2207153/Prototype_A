using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Transform[] _interactPoints; // 所有交互点
    public bool _touchObstacle;        // 是否触碰到障碍物
    private Transform _currentObstacle; // 当前触碰的障碍物引用

    public Transform GetInteractPoint(Transform playerTransform)
    {
        Transform interactPoint = null;

        float shortestDistance = float.PositiveInfinity;

        foreach (var point in _interactPoints)
        {
            // 仅选取未关闭的交互点
            if (point.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(point.position, playerTransform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    interactPoint = point;
                }
            }
        }

        return interactPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _touchObstacle = true;
            _currentObstacle = other.transform;
            Debug.Log("is is");
            // 关闭对应的交互点
            DisableInteractPointClosestToObstacle(_currentObstacle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _touchObstacle = false;
            _currentObstacle = null;
        }
    }

    private void DisableInteractPointClosestToObstacle(Transform obstacle)
    {
        float shortestDistance = float.PositiveInfinity;
        Transform closestPoint = null;

        // 找到距离障碍物最近的交互点
        foreach (var point in _interactPoints)
        {
            float distance = Vector3.Distance(point.position, obstacle.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPoint = point;
            }
        }

        // 禁用最近的交互点
        if (closestPoint != null)
        {
            closestPoint.gameObject.SetActive(false);
        }
    }
}
