using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Transform[] _interactPoints; // 箱の交互ポイント
    public float moveSpeed = 2f; // 移動速度
    public LayerMask obstacleLayer; // 障害物のレイヤー

    private bool isInteracting = false;
    private Transform interactPoint;
    private PlayerMovement playerController;

    public void StartInteraction(PlayerMovement player)
    {
        playerController = player;
        isInteracting = true;
    }

    public void StopInteraction()
    {
        isInteracting = false;
        playerController = null;
    }

    public Transform GetClosestInteractPoint(Transform playerTransform)
    {
        Transform closestPoint = null;
        float shortestDistance = float.PositiveInfinity;

        foreach (var point in _interactPoints)
        {
            float distance = Vector3.Distance(point.position, playerTransform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPoint = point;
            }
        }
        return closestPoint;
    }

    public void MoveBox(float input)
    {
        if (!isInteracting || input == 0) return;

        Vector3 moveDirection = interactPoint.forward * input * moveSpeed * Time.deltaTime;
        Vector3 targetPosition = input > 0 ? moveDirection : -moveDirection;

        // 障害物をチェックしながら移動
        if (!CheckObstacle(targetPosition))
        {
            transform.position += targetPosition;
        }
    }

    private bool CheckObstacle(Vector3 direction)
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, obstacleLayer);
    }
}
